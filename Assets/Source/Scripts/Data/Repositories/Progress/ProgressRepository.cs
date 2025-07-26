using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : IProgressRepository, IDisposable
    {
        public PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; }
        public PersistentReactiveProperty<int> NewWordsDailyTarget { get; }
        public PersistentReactiveProperty<int> CurrentStreak { get; }
        public PersistentReactiveProperty<int> BestStreak { get; }

        public PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; }

        public int NewWordsCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.NewWordsCount
            : 0;

        public int ReviewCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.ReviewCount
            : 0;

        internal ProgressRepository()
        {
            NewWordsDailyTarget = new PersistentReactiveProperty<int>(PersistentPropertyKeys.DailyGoalKey);
            CurrentStreak = new PersistentReactiveProperty<int>(PersistentPropertyKeys.CurrentStreakKey);
            BestStreak = new PersistentReactiveProperty<int>(PersistentPropertyKeys.BestStreakKey);
            ProgressHistory = new PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>>(
                PersistentPropertyKeys.ProgressEntryKey, new Dictionary<DateTime, DailyProgress>());

            TotalCountByState = new PersistentReactiveProperty<EnumArray<LearningState, int>>(
                PersistentPropertyKeys.TotalCountByStateKey, new EnumArray<LearningState, int>(EnumMode.SkipFirst));

            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            ProgressHistory.Value.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                CurrentStreak.Value = 0;
        }

        public void AddProgressToEntry(LearningState learningState, DateTime date)
        {
            var dateOnly = date.Date;

            if (ProgressHistory.Value.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new DailyProgress(dateOnly);
                ProgressHistory.Value[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);

            if (learningState == LearningState.CurrentlyLearning)
                ProcessNewWordProgress(ref dailyProgress);

            ProgressHistory.Value[dateOnly] = dailyProgress;
            IncreaseTotalCount(learningState);
        }

        public void IncrementNewWordsCount()
        {
            var today = DateTime.Now.Date;
            if (ProgressHistory.Value.TryGetValue(today, out var todayProgress) is false)
            {
                todayProgress = new DailyProgress(today);
                ProgressHistory.Value[today] = todayProgress;
            }

            todayProgress.NewWordsCount++;
            ProgressHistory.Value[today] = todayProgress;
        }

        public void IncrementReviewCount()
        {
            var today = DateTime.Now.Date;
            if (ProgressHistory.Value.TryGetValue(today, out var todayProgress) is false)
            {
                todayProgress = new DailyProgress(today);
                ProgressHistory.Value[today] = todayProgress;
            }

            todayProgress.ReviewCount++;
            ProgressHistory.Value[today] = todayProgress;
        }

        private void ProcessNewWordProgress(ref DailyProgress dailyProgress)
        {
            if (dailyProgress.GoalAchieved || GetProgressForDailyGoal(ref dailyProgress) < NewWordsDailyTarget.Value)
                return;

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

        private int GetProgressForDailyGoal(ref DailyProgress dailyProgress)
            => dailyProgress.ProgressByState[LearningState.CurrentlyLearning];

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