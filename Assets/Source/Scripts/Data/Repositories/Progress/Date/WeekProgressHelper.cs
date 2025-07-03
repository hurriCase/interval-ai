using System;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.User;

namespace Source.Scripts.Data.Repositories.Progress.Date
{
    internal static class WeekProgressHelper
    {
        private static readonly DailyProgress[] _monthProgressData = new DailyProgress[42]; // 6 weeks * 7 days = 42
        private static readonly bool[] _isInMonth = new bool[42];
        private static int _lastMonth = -1;
        private static int _lastYear = -1;

        private static readonly DailyProgress[] _currentWeek = new DailyProgress[7];
        private static DateTime _lastWeekStart = DateTime.MinValue;

        private static string _lastCultureName;

        internal static DailyProgress[] GetCurrentWeek()
        {
            var today = DateTime.Now.Date;
            var currentCulture = UserRepository.Instance.UserEntry.Value.CurrentCulture;
            var currentWeekStart = GetFirstDayOfWeek(today);

            if (currentCulture.Name == _lastCultureName && currentWeekStart == _lastWeekStart &&
                _currentWeek.Length != 0)
                return _currentWeek;

            RefreshCurrentWeek();
            _lastWeekStart = currentWeekStart;
            _lastCultureName = currentCulture.Name;

            return _currentWeek;
        }

        internal static void RefreshCurrentWeek()
        {
            if (ShouldRefreshWeek() is false)
            {
                UpdateTodayInCache();
                return;
            }

            var today = DateTime.Now.Date;
            var firstDayOfWeek = GetFirstDayOfWeek(today);
            var progressEntry = ProgressRepository.Instance.ProgressEntry.Value;

            for (var i = 0; i < 7; i++)
            {
                var date = firstDayOfWeek.AddDays(i);
                _currentWeek[i] = progressEntry.ProgressHistory.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(date);
            }
        }

        internal static (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month)
        {
            if (_lastYear == year && _lastMonth == month)
                return (_monthProgressData, _isInMonth);

            var monthStart = new DateTime(year, month, 1);
            var firstWeekStart = WeekProgressHelper.GetFirstDayOfWeek(monthStart);
            var calendarEnd = firstWeekStart.AddDays(41);
            var progressEntry = ProgressRepository.Instance.ProgressEntry.Value;
            var dayIndex = 0;

            for (var date = firstWeekStart; date <= calendarEnd; date = date.AddDays(1))
            {
                _monthProgressData[dayIndex] = progressEntry.ProgressHistory.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(date);

                _isInMonth[dayIndex] = date.Month == month;

                dayIndex++;
            }

            _lastMonth = month;
            _lastYear = year;

            return (_monthProgressData, _isInMonth);
        }

        private static DateTime GetFirstDayOfWeek(DateTime date)
        {
            var daysFromFirstDay = GetDayIndexInWeek(date);
            return date.AddDays(-daysFromFirstDay);
        }

        private static void UpdateTodayInCache()
        {
            var today = DateTime.Now.Date;
            var todayIndex = GetDayIndexInWeek(today);
            var progressEntry = ProgressRepository.Instance.ProgressEntry.Value;

            if (progressEntry.ProgressHistory.TryGetValue(today, out var todayProgress))
                _currentWeek[todayIndex] = todayProgress;
        }

        private static bool ShouldRefreshWeek()
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetFirstDayOfWeek(today);
            return currentWeekStart != _lastWeekStart;
        }

        private static int GetDayIndexInWeek(DateTime date)
        {
            var culture = UserRepository.Instance.UserEntry.Value.CurrentCulture;
            var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            return ((int)date.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        }
    }
}