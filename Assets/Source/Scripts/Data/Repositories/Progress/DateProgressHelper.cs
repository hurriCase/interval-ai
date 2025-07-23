using System;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal sealed class DateProgressHelper : IDateProgressHelper
    {
        private static readonly DailyProgress[] _monthProgressData = new DailyProgress[42]; // 6 weeks * 7 days = 42
        private static readonly bool[] _isInMonth = new bool[42];
        private static int _lastMonth = -1;
        private static int _lastYear = -1;

        private static readonly DailyProgress[] _currentWeek = new DailyProgress[7];

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
            var progressEntry = ProgressRepository.Instance.ProgressHistory.Value;
            for (var i = 0; i < 7; i++)
            var progressEntry = _progressRepository.ProgressHistory.Value;
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
            var calendarEnd = firstWeekStart.AddDays(41);
            var progressEntry = ProgressRepository.Instance.ProgressHistory.Value;
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

            return ((int)date.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        }
    }
}