using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using ZLinq;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class ProgressRepository : Singleton<ProgressRepository>
    {
        internal PersistentReactiveProperty<Entries.ProgressEntry> ProgressEntry { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey,
                new Entries.ProgressEntry(DefaultDailyWordsGoal, new Dictionary<DateTime, Entries.DailyProgress>()));

        private const int DefaultDailyWordsGoal = 10;

        private static readonly List<Entries.DailyProgress> _currentWeek = new();
        private static DateTime _lastWeekStart = DateTime.MinValue;

        internal void Init()
        {
            ProgressEntry.Subscribe(this, static (entry, helper) => helper.OnProgressChanged(entry));
        }

        internal void AddProgress(LearningState learningState)
        {
            var currentEntry = ProgressEntry.Value;
            var dateOnly = DateTime.Now.Date;

            if (currentEntry.ProgressHistory.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new Entries.DailyProgress(
                    null,
                    dateOnly
                );
                currentEntry.ProgressHistory[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);

            currentEntry.ProgressHistory[dateOnly] = dailyProgress;

            ProgressEntry.Value = currentEntry;
        }

        internal List<Entries.DailyProgress> GetCurrentWeek()
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetMondayOfWeek(today);

            if (currentWeekStart == _lastWeekStart && _currentWeek.Count != 0)
                return _currentWeek;

            RefreshCurrentWeek();
            _lastWeekStart = currentWeekStart;

            return _currentWeek;
        }

        internal List<Entries.DailyProgress> GetMonth(int year, int month)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            return GetProgressForDateRange(monthStart, monthEnd);
        }

        private List<Entries.DailyProgress> GetProgressForDateRange(DateTime startDate, DateTime endDate)
        {
            var progressList = new List<Entries.DailyProgress>();
            var currentEntry = ProgressEntry.Value;

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (currentEntry.ProgressHistory.TryGetValue(date, out var progress))
                    progressList.Add(progress);
            }

            return progressList;
        }

        private void OnProgressChanged(Entries.ProgressEntry progressEntry)
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

        private void UpdateTodayInCache(DateTime today, Entries.ProgressEntry progressEntry)
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
                    : new Entries.DailyProgress(null, date));
            }
        }
    }
}