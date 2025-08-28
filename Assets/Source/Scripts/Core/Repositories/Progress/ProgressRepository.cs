using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    internal sealed partial class ProgressRepository : IProgressRepository, IRepository, IDisposable
    {
        public ReadOnlyReactiveProperty<int> CurrentStreak => _currentStreak.Property;
        public ReadOnlyReactiveProperty<int> BestStreak => _bestStreak.Property;
        public ReadOnlyReactiveProperty<int> NewWordsDailyTarget => _newWordsDailyTarget.Property;
        public ReadOnlyReactiveProperty<EnumArray<LearningState, int>> TotalCountByState => _totalCountByState.Property;
        public ReadOnlyReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory =>
            _progressHistory.Property;

        private readonly PersistentReactiveProperty<int> _currentStreak = new();
        private readonly PersistentReactiveProperty<int> _bestStreak = new();
        private readonly PersistentReactiveProperty<int> _newWordsDailyTarget = new();
        private readonly PersistentReactiveProperty<EnumArray<LearningState, int>> _totalCountByState = new();
        private readonly PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> _progressHistory = new();

        public EnumArray<PracticeState, ReadOnlyReactiveProperty<int>> LearnedWordCounts { get; }
            = new(EnumMode.SkipFirst);

        private readonly EnumArray<PracticeState, ReactiveProperty<int>> _learnedWordCounts =
            new(() => new ReactiveProperty<int>(), EnumMode.SkipFirst);

        public ReadOnlyReactiveProperty<bool> HasDailyTarget => _hasDailyTarget;
        private readonly ReactiveProperty<bool> _hasDailyTarget = new(false);

        public Observable<int> GoalAchievedObservable => _goalAchievedSubject.AsObservable();
        private readonly Subject<int> _goalAchievedSubject = new();

        private readonly IPracticeSettingsRepository _practiceSettingsRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IAppConfig _appConfig;

        private IDisposable _disposable;

        internal ProgressRepository(
            IStatisticsRepository statisticsRepository,
            IPracticeSettingsRepository practiceSettingsRepository,
            IAppConfig appConfig)
        {
            _practiceSettingsRepository = practiceSettingsRepository;
            _statisticsRepository = statisticsRepository;
            _appConfig = appConfig;

            foreach (var (practiceState, learnedCountProperty) in _learnedWordCounts.AsTuples())
            {
                var learnedWordCounts = LearnedWordCounts;
                learnedWordCounts[practiceState] = learnedCountProperty;
                LearnedWordCounts = learnedWordCounts;
            }
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                _currentStreak.InitAsync(PersistentKeys.CurrentStreakKey, token),
                _bestStreak.InitAsync(PersistentKeys.BestStreakKey, token),
                _newWordsDailyTarget.InitAsync(PersistentKeys.NewWordsDailyTargetKey, token),

                _totalCountByState.InitAsync(
                    PersistentKeys.TotalCountByStateKey,
                    token,
                    new EnumArray<LearningState, int>(EnumMode.SkipFirst)),

                _progressHistory.InitAsync(
                    PersistentKeys.ProgressHistoryKey,
                    token,
                    new Dictionary<DateTime, DailyProgress>())
            };

            await UniTask.WhenAll(initTasks);

            if (_statisticsRepository.LoginHistory.Value.TryGetValue(DateTime.Now, out _) is false)
                _newWordsDailyTarget.Value = _practiceSettingsRepository.DailyGoal.Value;

            _disposable = _newWordsDailyTarget
                .Subscribe(this, (newTarget, self) => self._hasDailyTarget.Value = newTarget > 0);

            CheckStreak();
        }

        public void IncrementDailyProgress(LearningState learningState, DateTime date)
        {
            var dailyProgress = GetOrCreateDailyProgress(date);

            dailyProgress.AddProgress(learningState);

            if (_appConfig.LearningStateForDailyGoal == learningState)
            {
                ChangeDailyTarget(-1);
                TryIncreaseStreak(ref dailyProgress);
            }

            CheckLearnedWordsChanges(learningState);

            _progressHistory.Value[date.Date] = dailyProgress;
            IncreaseTotalCount(learningState);
        }

        public ProgressMemento CreateMemento() => new(this);

        public void ChangeDailyTarget(int valueToAdd)
        {
            if (_hasDailyTarget.Value is false && valueToAdd < 0)
                return;

            _newWordsDailyTarget.Value += valueToAdd;
        }

        private void CheckLearnedWordsChanges(LearningState requestedLearningState)
        {
            foreach (var (practiceState, learningState) in _appConfig.TargetStateForLearnedWords.AsTuples())
            {
                if (requestedLearningState != learningState)
                    continue;

                var todayProgress = GetOrCreateDailyProgress(DateTime.Today);
                _learnedWordCounts[practiceState].OnNext(todayProgress.ProgressByState[learningState]);
            }
        }

        private void TryIncreaseStreak(ref DailyProgress dailyProgress)
        {
            if (dailyProgress.GoalAchieved)
                return;

            var targetCount = _practiceSettingsRepository.DailyGoal.Value;
            var currentCount = dailyProgress.ProgressByState[_appConfig.LearningStateForDailyGoal];
            if (currentCount < targetCount)
                return;

            _currentStreak.Value++;

            _bestStreak.Value = Math.Max(_bestStreak.Value, _currentStreak.Value);

            dailyProgress.GoalAchieved = true;
            _goalAchievedSubject.OnNext(currentCount);
        }

        private DailyProgress GetOrCreateDailyProgress(DateTime date)
        {
            if (_progressHistory.Value.TryGetValue(date, out var dailyProgress))
                return dailyProgress;

            dailyProgress = new DailyProgress(date);
            _progressHistory.Value[date] = dailyProgress;

            return dailyProgress;
        }

        private void CheckStreak()
        {
            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            _progressHistory.Value.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                _currentStreak.Value = 0;
        }

        private void IncreaseTotalCount(LearningState state)
        {
            var totalCountByState = _totalCountByState.Value;
            totalCountByState[state]++;
            _totalCountByState.Value = totalCountByState;
            _totalCountByState.Property.OnNext(totalCountByState);
        }

        public void Dispose()
        {
            _currentStreak.Dispose();
            _bestStreak.Dispose();
            _newWordsDailyTarget.Dispose();
            _totalCountByState.Dispose();
            _progressHistory.Dispose();
            _goalAchievedSubject.Dispose();
            _disposable.Dispose();
        }
    }
}