using System;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Entries;
using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Progress.Date;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : Singleton<ProgressRepository>, IDisposable
    {
        internal PersistentReactiveProperty<ProgressEntry> ProgressEntry { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey,
                new ProgressEntry(DefaultDailyWordsGoal, default, 0, 0, new Dictionary<DateTime, DailyProgress>()));

        private const int DefaultDailyWordsGoal = 10;
        private IDisposable _disposable;

        internal void Init()
        {
            var yesterdayDate = DateTime.Now.Date.AddDays(-1);
            ProgressEntry.Value.ProgressHistory.TryGetValue(yesterdayDate, out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                ProgressEntry.ModifyValue(entry => entry.CurrentStreak = 0);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            ProgressEntry.Dispose();
        }
    }
}