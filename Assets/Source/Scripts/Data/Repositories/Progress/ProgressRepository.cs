using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : IProgressRepository, IRepository
    {
        public PersistentReactiveProperty<int> CurrentStreak { get; } = new();
        public PersistentReactiveProperty<int> BestStreak { get; } = new();
        public PersistentReactiveProperty<int> NewWordsDailyTarget { get; } = new();
        public PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; } = new();
        public PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; } = new();

        public int NewWordsCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.NewWordsCount
            : 0;

        public int ReviewCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.ReviewCount
            : 0;

        private DailyProgress _todayProgress;

        private readonly ISettingsRepository _settingsRepository;
        private readonly IStatisticsRepository _statisticsRepository;

        internal ProgressRepository(IStatisticsRepository statisticsRepository, ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _statisticsRepository = statisticsRepository;
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

            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            ProgressHistory.Value.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                CurrentStreak.Value = 0;
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

        public void IncrementNewWordsCount()
        {
            var today = DateTime.Now.Date;
            var todayProgress = GetOrCreateDailyProgress(today);

            todayProgress.NewWordsCount++;

            if (todayProgress.GoalAchieved is false &&
                todayProgress.NewWordsCount >= _settingsRepository.DailyGoal.Value)
                IncreaseStreak(ref todayProgress);

            ProgressHistory.Value[today] = todayProgress;
        }

        public void IncrementReviewCount()
        {
            var today = DateTime.Now.Date;
            var todayProgress = GetOrCreateDailyProgress(today);

            todayProgress.ReviewCount++;
            ProgressHistory.Value[today] = todayProgress;
        }

        private DailyProgress GetOrCreateDailyProgress(DateTime date)
        {
            if (ProgressHistory.Value.TryGetValue(date, out var dailyProgress))
                return dailyProgress;

            dailyProgress = new DailyProgress(date);
            ProgressHistory.Value[date] = dailyProgress;

            return dailyProgress;
        }

        private void IncreaseStreak(ref DailyProgress dailyProgress)
        {
            CurrentStreak.Value++;

            if (CurrentStreak.Value > BestStreak.Value)
                BestStreak.Value = CurrentStreak.Value;

            dailyProgress.GoalAchieved = true;
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