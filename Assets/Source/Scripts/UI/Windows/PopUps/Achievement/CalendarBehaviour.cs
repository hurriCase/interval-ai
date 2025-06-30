using System;
using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.UI.Windows.Shared;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement
{
    internal sealed class CalendarBehaviour : MonoBehaviour
    {
        [SerializeField] private List<WeekProgressContainer> _weekProgressContainers;

        internal void Init()
        {
            var now = DateTime.Now;
            var currentMonthProgress = ProgressRepository.Instance.GetMonth(now.Year, now.Month);

            var weeklyGroups = GroupByCalendarWeeks(currentMonthProgress, now.Year, now.Month);

            for (var i = 0; i < weeklyGroups.Count && i < _weekProgressContainers.Count; i++)
            {
                var weekGroup = weeklyGroups[i];
                var monthDates = weekGroup.Select(progress => progress.DateTime.Day.ToString()).ToList();

                _weekProgressContainers[i].UpdateWeeklyProgress(weekGroup, monthDates);
            }
        }

        //TODO:<Dmitriy.Sukharev> refactor
        private List<List<DailyProgress>> GroupByCalendarWeeks(List<DailyProgress> monthProgress, int year, int month)
        {
            var progressByDate =
                monthProgress.ToDictionary(dailyProgress => dailyProgress.DateTime.Date, dailyProgress => dailyProgress);

            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var weeks = new List<List<DailyProgress>>();

            var currentWeekStart = GetMondayOfWeek(firstDayOfMonth);

            while (currentWeekStart <= lastDayOfMonth)
            {
                var weekDays = new List<DailyProgress>();

                for (var dayOffset = 0; dayOffset < 7; dayOffset++)
                {
                    var currentDate = currentWeekStart.AddDays(dayOffset);

                    if (currentDate.Month == month && currentDate.Year == year)
                        weekDays.Add(progressByDate.TryGetValue(currentDate, out var progress)
                            ? progress
                            : new DailyProgress(currentDate));
                }

                if (weekDays.Count > 0)
                    weeks.Add(weekDays);

                currentWeekStart = currentWeekStart.AddDays(7);
            }

            return weeks;
        }

        private DateTime GetMondayOfWeek(DateTime date)
        {
            var daysFromMonday = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-daysFromMonday);
        }
    }
}