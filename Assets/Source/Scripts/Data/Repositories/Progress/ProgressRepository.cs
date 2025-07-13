using System;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Entries;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : Singleton<ProgressRepository>, IDisposable
    {
        internal PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; } =
            new(PersistentPropertyKeys.TotalCountByStateKey);
        internal PersistentReactiveProperty<int> DailyWordsGoal { get; } =
            new(PersistentPropertyKeys.DailyGoalKey, DefaultDailyWordsGoal);

        internal PersistentReactiveProperty<int> CurrentStreak { get; } = new(PersistentPropertyKeys.CurrentStreakKey);
        internal PersistentReactiveProperty<int> BestStreak { get; } = new(PersistentPropertyKeys.BestStreakKey);

        internal PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey, new Dictionary<DateTime, DailyProgress>());

        private const int DefaultDailyWordsGoal = 10;
        private IDisposable _disposable;

        internal void Init()
        {
            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            ProgressHistory.Value.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                CurrentStreak.Value = 0;
        }

        internal void IncreaseTotalCount(LearningState state)
        {
            var totalCountByState = TotalCountByState.Value;
            totalCountByState[state]++;
            TotalCountByState.Value = totalCountByState;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            ProgressHistory.Dispose();
        }
    }
}