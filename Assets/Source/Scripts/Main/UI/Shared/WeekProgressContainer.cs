using System.Collections.Generic;
using Source.Scripts.Core.DI.Repositories.Progress;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using Source.Scripts.Core.DI.Repositories.Settings.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class WeekProgressContainer : MonoBehaviour
    {
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ActivityMapping _activityMapping;
        [SerializeField] private List<ProgressItem> _progressItems;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private IDateProgressHelper _dateProgressHelper;

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
                    _activityMapping,
                    isOutsideMonth);
            }
        }

        internal void UpdateCurrentWeeklyProgress()
        {
            var weekAbbreviatedNames = _settingsRepository.CurrentCulture.Value.DateTimeFormat.AbbreviatedDayNames;
            var currentWeek = _dateProgressHelper.GetCurrentWeek();

            for (var day = 0; day < 7; day++)
            {
                var dailyProgress = currentWeek[day];
                var dayText = weekAbbreviatedNames[day];

                _progressItems[day].Init(dailyProgress.ProgressByState, dayText, _progressColorMapping);
            }
        }
    }
}