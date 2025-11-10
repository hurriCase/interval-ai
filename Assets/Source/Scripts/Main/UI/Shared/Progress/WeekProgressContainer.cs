using System.Collections.Generic;
using CustomUtils.Runtime.Constants;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class WeekProgressContainer : MonoBehaviour
    {
        [SerializeField] private List<ProgressItem> _progressItems;

        private IUISettingsRepository _uiSettingsRepository;
        private IDateProgressService _dateProgressService;

        [Inject]
        internal void Inject(IUISettingsRepository uiSettingsRepository, IDateProgressService dateProgressService)
        {
            _uiSettingsRepository = uiSettingsRepository;
            _dateProgressService = dateProgressService;
        }

        internal void UpdateCurrentWeeklyProgress()
        {
            var weekAbbreviatedNames =
                _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat.AbbreviatedDayNames;

            var currentWeek = _dateProgressService.GetCurrentWeek();

            for (var day = 0; day < Date.DaysPerWeek; day++)
            {
                var dailyProgress = currentWeek[day];
                var dayText = weekAbbreviatedNames[day];

                _progressItems[day].Init(dailyProgress.ProgressByState, dayText);
            }
        }
    }
}