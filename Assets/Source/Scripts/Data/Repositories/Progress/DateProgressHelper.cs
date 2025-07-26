using System;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class DateProgressHelper : IDateProgressHelper
    {
        private readonly DailyProgress[] _monthProgressData = new DailyProgress[CalendarWeeks * DaysPerWeek];
        private readonly bool[] _isInMonth = new bool[CalendarWeeks * DaysPerWeek];
        private int _lastMonth = -1;
        private int _lastYear = -1;

        private const int CalendarWeeks = 6;
        private const int DaysPerWeek = 7;

        private readonly DailyProgress[] _currentWeek = new DailyProgress[DaysPerWeek];

        private readonly IProgressRepository _progressRepository;
        private readonly IUserRepository _userRepository;

        internal DateProgressHelper(IProgressRepository progressRepository, IUserRepository userRepository)
        {
            _progressRepository = progressRepository;
            _userRepository = userRepository;
        }

        public DailyProgress[] GetCurrentWeek()
        {
            var today = DateTime.Now.Date;
            var currentWeekStart = GetFirstDayOfWeek(today);
            var progressEntry = _progressRepository.ProgressHistory.Value;
            for (var i = 0; i < DaysPerWeek; i++)
            {
                var date = currentWeekStart.AddDays(i);
                _currentWeek[i] = progressEntry.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(date);
            }

            return _currentWeek;
        }

        public (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month)
        {
            if (_lastYear == year && _lastMonth == month)
                return (_monthProgressData, _isInMonth);

            var monthStart = new DateTime(year, month, 1);
            var firstWeekStart = GetFirstDayOfWeek(monthStart);
            var calendarEnd = firstWeekStart.AddDays(CalendarWeeks * DaysPerWeek - 1);
            var progressEntry = _progressRepository.ProgressHistory.Value;
            var dayIndex = 0;

            for (var date = firstWeekStart; date <= calendarEnd; date = date.AddDays(1))
            {
                _monthProgressData[dayIndex] = progressEntry.TryGetValue(date, out var progress)
                    ? progress
                    : new DailyProgress(date);

                _isInMonth[dayIndex] = date.Month == month;

                dayIndex++;
            }

            _lastMonth = month;
            _lastYear = year;

            return (_monthProgressData, _isInMonth);
        }

        public int GetProgressForRange(int daysBack, int daysDuration, LearningState learningState)
        {
            var endDate = DateTime.Now.Date.AddDays(-daysBack);
            var startDate = endDate.AddDays(-daysDuration + 1);

            var progressEntry = _progressRepository.ProgressHistory.Value;
            var totalProgress = 0;

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (progressEntry.TryGetValue(date, out var dailyProgress))
                    totalProgress += dailyProgress.GetProgressCountData(learningState);
            }

            return totalProgress;
        }

        private DateTime GetFirstDayOfWeek(DateTime date)
        {
            var daysFromFirstDay = GetDayIndexInWeek(date);
            return date.AddDays(-daysFromFirstDay);
        }

        private int GetDayIndexInWeek(DateTime date)
        {
            var firstDayOfWeek = _userRepository.CurrentCulture.Value.DateTimeFormat.FirstDayOfWeek;

            return ((int)date.DayOfWeek - (int)firstDayOfWeek + DaysPerWeek) % DaysPerWeek;
        }
    }
}