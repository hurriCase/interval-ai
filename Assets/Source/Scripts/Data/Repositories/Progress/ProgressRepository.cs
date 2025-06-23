using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using ZLinq;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : Singleton<ProgressRepository>, IDisposable
    {
        internal PersistentReactiveProperty<ProgressEntry> ProgressEntry { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey,
                new ProgressEntry(DefaultDailyWordsGoal, null, 0, 0, new Dictionary<DateTime, DailyProgress>()));

        private const int DefaultDailyWordsGoal = 10;

        private static readonly List<DailyProgress> _currentWeek = new();
        private static DateTime _lastWeekStart = DateTime.MinValue;

        private IDisposable _disposable;

        internal void Init()
        {
            _disposable =
                ProgressEntry.Subscribe(this, static (entry, helper) => helper.OnProgressChanged(entry));

            ProgressEntry.Value.ProgressHistory.TryGetValue(DateTime.Now.AddDays(-1), out var lastDayProgress);

            if (lastDayProgress.GoalAchieved is false)
                ProgressEntry.ModifyValue(entry => entry.CurrentStreak = 0);
        }

        internal void AddProgress(LearningState learningState)
        {
            ProgressEntry.ModifyValue((repository: this, learningState),
                (tuple, entry) => tuple.repository.AddProgressInternal(tuple.learningState, entry));
        }

        private void AddProgressInternal(LearningState learningState, ProgressEntry progressEntry)
        {
            var dateOnly = DateTime.Now.Date;

            if (progressEntry.ProgressHistory.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new DailyProgress(dateOnly);
                progressEntry.ProgressHistory[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);

            if (learningState == LearningState.CurrentlyLearning)
                HandleNewWord(ref dailyProgress, progressEntry);

            progressEntry.ProgressHistory[dateOnly] = dailyProgress;
            progressEntry.StateCounts[(int)learningState]++;
        }

        private void HandleNewWord(ref DailyProgress dailyProgress, ProgressEntry progressEntry)
        {
            var progressCount = dailyProgress.ProgressCountData[(int)LearningState.CurrentlyLearning];

            if (dailyProgress.GoalAchieved || progressCount < progressEntry.DailyWordsGoal)
                return;

            progressEntry.CurrentStreak++;

            if (progressEntry.CurrentStreak > progressEntry.BestStreak)
                progressEntry.BestStreak = progressEntry.CurrentStreak;

            dailyProgress.GoalAchieved = true;
        }

        internal List<DailyProgress> GetCurrentWeek()
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetMondayOfWeek(today);

            if (currentWeekStart == _lastWeekStart && _currentWeek.Count != 0)
                return _currentWeek;

            RefreshCurrentWeek();
            _lastWeekStart = currentWeekStart;

            return _currentWeek;
        }

        internal List<DailyProgress> GetMonth(int year, int month)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            return GetProgressForDateRange(monthStart, monthEnd);
        }

        private List<DailyProgress> GetProgressForDateRange(DateTime startDate, DateTime endDate)
        {
            var progressList = new List<DailyProgress>();
            var currentEntry = ProgressEntry.Value;

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (currentEntry.ProgressHistory.TryGetValue(date, out var progress))
                    progressList.Add(progress);
            }

            return progressList;
        }

        private void OnProgressChanged(ProgressEntry progressEntry)
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetMondayOfWeek(today);

            if (currentWeekStart != _lastWeekStart)
            {
                RefreshCurrentWeek();
                _lastWeekStart = currentWeekStart;
                return;
            }

            UpdateTodayInCache(today, progressEntry);
        }

        private DateTime GetMondayOfWeek(DateTime date)
        {
            var daysFromMonday = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-daysFromMonday);
        }

        private void UpdateTodayInCache(DateTime today, ProgressEntry progressEntry)
        {
            var todayProgress = progressEntry.ProgressHistory.AsValueEnumerable()
                .FirstOrDefault(progressHistory => progressHistory.Value.DateTime.Date == today);

            if (todayProgress.Value.DateTime == default)
                return;

            _currentWeek.RemoveAll(dailyProgress => dailyProgress.DateTime.Date == today);
            _currentWeek.Add(todayProgress.Value);
        }

        private void RefreshCurrentWeek()
        {
            _currentWeek.Clear();
            var today = DateTime.Now.Date;
            var monday = GetMondayOfWeek(today);

            for (var i = 0; i < 7; i++)
            {
                var date = monday.AddDays(i);
                _currentWeek.Add(ProgressEntry.Value.ProgressHistory.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(null, false, date));
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            ProgressEntry.Dispose();
        }
    }
}