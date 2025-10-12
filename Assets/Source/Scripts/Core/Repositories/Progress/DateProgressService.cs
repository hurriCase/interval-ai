using System;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    internal sealed class DateProgressService : IDateProgressService
    {
        private const int CalendarWeeks = 6;
        private const int DaysPerWeek = 7;

        private readonly DailyProgress[] _monthProgressData = new DailyProgress[CalendarWeeks * DaysPerWeek];
        private readonly bool[] _isInMonth = new bool[CalendarWeeks * DaysPerWeek];
        private readonly DailyProgress[] _currentWeek = new DailyProgress[DaysPerWeek];

        private int _lastMonth = -1;
        private int _lastYear = -1;

        private readonly IProgressRepository _progressRepository;
        private readonly IUISettingsRepository _uiSettingsRepository;

        internal DateProgressService(
            IProgressRepository progressRepository,
            IUISettingsRepository uiSettingsRepository)
        {
            _progressRepository = progressRepository;
            _uiSettingsRepository = uiSettingsRepository;
        }

        public DailyProgress[] GetCurrentWeek()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var weekStart = GetFirstDayOfWeek(today);

            for (var i = 0; i < DaysPerWeek; i++)
            {
                var date = weekStart.AddDays(i);
                _currentWeek[i] = GetProgressForDateOrDefault(date);
            }

            return _currentWeek;
        }

        public (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month)
        {
            if (_lastYear == year && _lastMonth == month)
                return (_monthProgressData, _isInMonth);

            var monthStart = new DateOnly(year, month, 1);
            var firstWeekStart = GetFirstDayOfWeek(monthStart);
            var calendarEnd = firstWeekStart.AddDays(CalendarWeeks * DaysPerWeek - 1);
            var dayIndex = 0;

            for (var date = firstWeekStart; date <= calendarEnd; date = date.AddDays(1))
            {
                _monthProgressData[dayIndex] = GetProgressForDateOrDefault(date);
                _isInMonth[dayIndex] = date.Month == month;
                dayIndex++;
            }

            _lastMonth = month;
            _lastYear = year;

            return (_monthProgressData, _isInMonth);
        }

        public int GetProgressForRange(int daysBack, int daysDuration, LearningState learningState)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var endDate = now.AddDays(-daysBack);
            var startDate = endDate.AddDays(-daysDuration + 1);
            var progressEntry = _progressRepository.ProgressHistory.CurrentValue;
            var totalProgress = 0;

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (progressEntry.TryGetValue(date, out var dailyProgress))
                    totalProgress += dailyProgress.GetProgressCountData(learningState);
            }

            return totalProgress;
        }

        private DailyProgress GetProgressForDateOrDefault(DateOnly date)
        {
            var progressEntry = _progressRepository.ProgressHistory.CurrentValue;

            return progressEntry.TryGetValue(date, out var progress)
                ? progress
                : new DailyProgress(date);
        }

        private DateOnly GetFirstDayOfWeek(DateOnly date)
        {
            var daysFromFirstDay = GetDayIndexInWeek(date);
            return date.AddDays(-daysFromFirstDay);
        }

        private int GetDayIndexInWeek(DateOnly date)
        {
            var firstDayOfWeek = _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat.FirstDayOfWeek;
            return ((int)date.DayOfWeek - (int)firstDayOfWeek + DaysPerWeek) % DaysPerWeek;
        }
    }
}