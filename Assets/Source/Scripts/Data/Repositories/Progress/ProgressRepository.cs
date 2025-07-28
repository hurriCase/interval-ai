using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Statistics;
using Source.Scripts.Data.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : IProgressRepository, IDisposable
    {
        public PersistentReactiveProperty<int> CurrentStreak { get; }
        public PersistentReactiveProperty<int> BestStreak { get; }
        public PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; }
        public PersistentReactiveProperty<int> NewWordsDailyTarget { get; }
        public PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; }

        public int NewWordsCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.NewWordsCount
            : 0;

        public int ReviewCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.ReviewCount
            : 0;

        private DailyProgress _todayProgress;

        private readonly ISettingsRepository _settingsRepository;

        internal ProgressRepository(IStatisticsRepository statisticsRepository, ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;

            CurrentStreak = new PersistentReactiveProperty<int>(PersistentPropertyKeys.CurrentStreakKey);
            BestStreak = new PersistentReactiveProperty<int>(PersistentPropertyKeys.BestStreakKey);
            TotalCountByState = new PersistentReactiveProperty<EnumArray<LearningState, int>>(
                PersistentPropertyKeys.TotalCountByStateKey, new EnumArray<LearningState, int>(EnumMode.SkipFirst));

            NewWordsDailyTarget = new PersistentReactiveProperty<int>(PersistentPropertyKeys.NewWordsDailyTargetKey);

            if (statisticsRepository.LoginHistory.Value.TryGetValue(DateTime.Now, out _) is false)
                NewWordsDailyTarget.Value = settingsRepository.DailyGoal.Value;

            ProgressHistory = new PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>>(
                PersistentPropertyKeys.ProgressHistoryKey, new Dictionary<DateTime, DailyProgress>());

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