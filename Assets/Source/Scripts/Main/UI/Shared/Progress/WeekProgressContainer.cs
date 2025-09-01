using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.Shared.Activity;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class WeekProgressContainer : MonoBehaviour
    {
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ActivityMapping _activityMapping;
        [SerializeField] private List<ProgressItem> _progressItems;

        private IUISettingsRepository _uiSettingsRepository;
        private IDateProgressService _dateProgressService;

        [Inject]
        internal void Inject(IUISettingsRepository uiSettingsRepository, IDateProgressService dateProgressService)
        {
            _uiSettingsRepository = uiSettingsRepository;
            _dateProgressService = dateProgressService;
        }

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
            var weekAbbreviatedNames = _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat.AbbreviatedDayNames;
            var currentWeek = _dateProgressService.GetCurrentWeek();

            for (var day = 0; day < 7; day++)
            {
                var dailyProgress = currentWeek[day];
                var dayText = weekAbbreviatedNames[day];

                _progressItems[day].Init(dailyProgress.ProgressByState, dayText, _progressColorMapping);
            }
        }
    }
}