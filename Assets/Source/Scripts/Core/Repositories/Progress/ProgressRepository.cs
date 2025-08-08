using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    internal sealed class ProgressRepository : IProgressRepository, IRepository
    {
        public PersistentReactiveProperty<int> CurrentStreak { get; } = new();
        public PersistentReactiveProperty<int> BestStreak { get; } = new();
        public PersistentReactiveProperty<int> NewWordsDailyTarget { get; } = new();
        public PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; } = new();
        public PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; } = new();

        public EnumArray<PracticeState, Observable<int>> LearnedWordCountObservables { get; } = new(EnumMode.SkipFirst);
        private readonly EnumArray<PracticeState, Subject<int>> _learnedWordCountSubjects = new(EnumMode.SkipFirst);

        public Observable<Unit> GoalAchievedObservable => _goalAchievedSubject.AsObservable();
        private readonly Subject<Unit> _goalAchievedSubject = new();

        private DailyProgress _todayProgress;

        private readonly ISettingsRepository _settingsRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        internal ProgressRepository(IStatisticsRepository statisticsRepository, ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _statisticsRepository = statisticsRepository;

            foreach (var (practiceState, subject) in _learnedWordCountSubjects.AsTuples())
            {
                var learnedWordCountObservables = LearnedWordCountObservables;
                learnedWordCountObservables[practiceState] = subject.AsObservable();
                LearnedWordCountObservables = learnedWordCountObservables;
            }
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                CurrentStreak.InitAsync(PersistentKeys.CurrentStreakKey, cancellationToken),
                BestStreak.InitAsync(PersistentKeys.BestStreakKey, cancellationToken),
                NewWordsDailyTarget.InitAsync(PersistentKeys.NewWordsDailyTargetKey, cancellationToken),

                TotalCountByState.InitAsync(
                    PersistentKeys.TotalCountByStateKey,
                    cancellationToken,
                    new EnumArray<LearningState, int>(EnumMode.SkipFirst)),

                ProgressHistory.InitAsync(
                    PersistentKeys.ProgressHistoryKey,
                    cancellationToken,
                    new Dictionary<DateTime, DailyProgress>())
            };

            await UniTask.WhenAll(initTasks);

            if (_statisticsRepository.LoginHistory.Value.TryGetValue(DateTime.Now, out _) is false)
                NewWordsDailyTarget.Value = _settingsRepository.DailyGoal.Value;

            CheckStreak();
        }

        public void IncrementDailyProgress(LearningState learningState, DateTime date)
        {
            var dailyProgress = GetOrCreateDailyProgress(date);

            dailyProgress.AddProgress(learningState);

            if (learningState == LearningState.CurrentlyLearning)
                IncreaseStreak(ref dailyProgress);

            ProgressHistory.Value[date.Date] = dailyProgress;
            IncreaseTotalCount(learningState);
        }

        public void IncrementLearnedWordsCount(PracticeState practiceState)
        {
            var today = DateTime.Now.Date;
            var todayProgress = GetOrCreateDailyProgress(today);

            var todayProgressWordsCount = todayProgress.LearnedWordsCount;
            todayProgressWordsCount[practiceState]++;
            todayProgress.LearnedWordsCount = todayProgressWordsCount;

            if (practiceState == PracticeState.NewWords && todayProgress.GoalAchieved is false &&
                todayProgress.LearnedWordsCount[PracticeState.NewWords] >= _settingsRepository.DailyGoal.Value)
                IncreaseStreak(ref todayProgress);

            ProgressHistory.Value[today] = todayProgress;

            var newWordsCount = todayProgress.LearnedWordsCount[PracticeState.NewWords];
            _learnedWordCountSubjects[practiceState].OnNext(newWordsCount);
        }

        private DailyProgress GetOrCreateDailyProgress(DateTime date)
        {
            if (ProgressHistory.Value.TryGetValue(date, out var dailyProgress))
                return dailyProgress;

            dailyProgress = new DailyProgress(date);
            ProgressHistory.Value[date] = dailyProgress;

            return dailyProgress;
        }

        private void CheckStreak()
        {
            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            ProgressHistory.Value.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                CurrentStreak.Value = 0;
        }

        private void IncreaseStreak(ref DailyProgress dailyProgress)
        {
            CurrentStreak.Value++;

            if (CurrentStreak.Value > BestStreak.Value)
                BestStreak.Value = CurrentStreak.Value;

            if (dailyProgress.GoalAchieved)
                return;

            dailyProgress.GoalAchieved = true;
            _goalAchievedSubject.OnNext(Unit.Default);
        }

        private void IncreaseTotalCount(LearningState state)
        {
            var totalCountByState = TotalCountByState.Value;
            totalCountByState[state]++;
            TotalCountByState.Value = totalCountByState;
            TotalCountByState.Property.OnNext(totalCountByState);
        }

        public void Dispose()
        {
            TotalCountByState.Dispose();
            NewWordsDailyTarget.Dispose();
            CurrentStreak.Dispose();
            BestStreak.Dispose();
            ProgressHistory.Dispose();
        }
    }
}