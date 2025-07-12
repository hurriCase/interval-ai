using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Progress.Date;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    internal sealed class WeekProgressContainer : MonoBehaviour
    {
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private DateIdentifierMapping _dateIdentifierMapping;
        [SerializeField] private List<ProgressItem> _progressItems;

        internal void UpdateMonthWeeklyProgress(DailyProgress[] monthData, int weekIndex, bool[] isInMonth)
        {
            var weekStart = weekIndex * 7;

            for (var day = 0; day < 7; day++)
            {
                var dayIndex = weekStart + day;
                var dailyProgress = monthData[dayIndex];
                var dayText = dailyProgress.DateTime.Day.ToString();
                var isOutsideMonth = isInMonth[dayIndex] is false;

                _progressItems[day].Init(
                    dailyProgress.ProgressByState,
                    dayText,
                    _progressColorMapping,
                    _dateIdentifierMapping,
                    isOutsideMonth);
            }
        }

        internal void UpdateCurrentWeeklyProgress()
        {
            var weekAbbreviatedNames = UserRepository.Instance.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            var currentWeek = DateProgressHelper.GetCurrentWeek();

            for (var day = 0; day < 7; day++)
            {
                var dailyProgress = currentWeek[day];
                var dayText = weekAbbreviatedNames[day];

                _progressItems[day].Init(dailyProgress.ProgressByState, dayText, _progressColorMapping);
            }
        }
    }
}