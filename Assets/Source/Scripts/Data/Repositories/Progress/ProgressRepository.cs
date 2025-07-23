using System;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Entries;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : IProgressRepository, IDisposable
    {
        public PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; } =
            new(PersistentPropertyKeys.TotalCountByStateKey, new EnumArray<LearningState, int>(EnumMode.SkipFirst));
        public PersistentReactiveProperty<int> DailyWordsGoal { get; } =
            new(PersistentPropertyKeys.DailyGoalKey, DefaultDailyWordsGoal);

        public PersistentReactiveProperty<int> CurrentStreak { get; } = new(PersistentPropertyKeys.CurrentStreakKey);
        public PersistentReactiveProperty<int> BestStreak { get; } = new(PersistentPropertyKeys.BestStreakKey);

        public PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey, new Dictionary<DateTime, DailyProgress>());

        public int NewWordsCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.NewWordsCount
            : 0;

        public int ReviewCount => ProgressHistory.Value.TryGetValue(DateTime.Now.Date, out var todayProgress)
            ? todayProgress.ReviewCount
            : 0;

        private const int DefaultDailyWordsGoal = 10;
        private IDisposable _disposable;

        internal ProgressRepository()
        {
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

        public void IncreaseTotalCount(LearningState state)
        {
            var totalCountByState = TotalCountByState.Value;
            totalCountByState[state]++;
            TotalCountByState.Value = totalCountByState;
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

        public void Dispose()
        {
            _disposable?.Dispose();
            ProgressHistory.Dispose();
        }

        private void ProcessNewWordProgress(ref DailyProgress dailyProgress)
        {
            if (dailyProgress.GoalAchieved || GetProgressForDailyGoal(ref dailyProgress) < DailyWordsGoal.Value)
                return;

            CurrentStreak.Value++;

            if (CurrentStreak.Value > BestStreak.Value)
                BestStreak.Value = CurrentStreak.Value;

            dailyProgress.GoalAchieved = true;
        }

        private int GetProgressForDailyGoal(ref DailyProgress dailyProgress)
            => dailyProgress.ProgressByState[LearningState.CurrentlyLearning];
    }
}