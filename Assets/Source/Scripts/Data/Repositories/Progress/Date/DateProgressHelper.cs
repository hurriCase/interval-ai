using System;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Progress.Date
{
    internal static class DateProgressHelper
    {
        private static readonly DailyProgress[] _monthProgressData = new DailyProgress[42]; // 6 weeks * 7 days = 42
        private static readonly bool[] _isInMonth = new bool[42];
        private static int _lastMonth = -1;
        private static int _lastYear = -1;

        private static readonly DailyProgress[] _currentWeek = new DailyProgress[7];

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatic()
        {
            _lastMonth = -1;
            _lastYear = -1;

            Array.Clear(_monthProgressData, 0, _monthProgressData.Length);
            Array.Clear(_isInMonth, 0, _isInMonth.Length);
            Array.Clear(_currentWeek, 0, _currentWeek.Length);
        }

        internal static DailyProgress[] GetCurrentWeek()
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetFirstDayOfWeek(today);
            var progressEntry = ProgressRepository.Instance.ProgressEntry.Value;
            for (var i = 0; i < 7; i++)
            {
                var date = currentWeekStart.AddDays(i);
                _currentWeek[i] = progressEntry.ProgressHistory.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(date);
            }

            return _currentWeek;
        }

        internal static (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month)
        {
            if (_lastYear == year && _lastMonth == month)
                return (_monthProgressData, _isInMonth);

            var monthStart = new DateTime(year, month, 1);
            var firstWeekStart = GetFirstDayOfWeek(monthStart);
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

        private static int GetDayIndexInWeek(DateTime date)
        {
            var culture = UserRepository.Instance.UserEntry.Value.CurrentCulture;
            var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

            return ((int)date.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        }
    }
}