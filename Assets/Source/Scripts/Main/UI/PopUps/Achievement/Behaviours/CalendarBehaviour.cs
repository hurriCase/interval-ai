using System;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using Source.Scripts.UI.Behaviours;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours
{
    internal sealed class CalendarBehaviour : MonoBehaviour
    {
        [SerializeField] private WeekLabelBehaviour _weekDaysBehaviour;
        [SerializeField] private TextMeshProUGUI _currentMonthText;
        [SerializeField] private ThemeButton _previousMonthButton;
        [SerializeField] private ThemeButton _nextMonthButton;
        [SerializeField] private CalendarProgress[] _calendarProgress = new CalendarProgress[DaysInCalendar];

        private const int DaysInCalendar = 42;

        private DateTime _currentDate = DateTime.Now;

        private IDateProgressService _dateProgressService;
        private IUISettingsRepository _uiSettingsRepository;

        [Inject]
        internal void Inject(IDateProgressService dateProgressService, IUISettingsRepository uiSettingsRepository)
        {
            _dateProgressService = dateProgressService;
            _uiSettingsRepository = uiSettingsRepository;
        }

        internal void Init()
        {
            _weekDaysBehaviour.Init();

            UpdateCalendarDisplay();

            _previousMonthButton.OnClickAsObservable()
                .Do(this, static self => self._currentDate = self._currentDate.AddMonths(-1))
                .SubscribeUntilDestroy(this, static self => self.UpdateCalendarDisplay());

            _nextMonthButton.OnClickAsObservable()
                .Do(this, static self => self._currentDate = self._currentDate.AddMonths(1))
                .SubscribeUntilDestroy(this, static self => self.UpdateCalendarDisplay());
        }

        private void UpdateCalendarDisplay()
        {
            var now = DateTime.Now;
            _nextMonthButton.interactable = _currentDate.Month != now.Month || _currentDate.Year != now.Year;

            var (monthData, isInMonth) =
                _dateProgressService.GetMonthWeeks(_currentDate.Year, _currentDate.Month);

            _currentMonthText.text = _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat
                .GetMonthName(_currentDate.Month);

            for (var day = 0; day < DaysInCalendar; day++)
            {
                var dailyProgress = monthData[day];
                var dayText = dailyProgress.Date.Day.ToString();
                var isOutsideMonth = isInMonth[day] is false;

                _calendarProgress[day].Init(dailyProgress.ProgressByState, dayText, isOutsideMonth);
            }
        }
    }
}