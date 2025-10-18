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

        private const int MonthsInYear = 12;
        private const int DaysInCalendar = 42;

        private int _currentYear;
        private int _currentMonth;

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

            var now = DateTime.Now;
            _currentYear = now.Year;
            _currentMonth = now.Month;

            UpdateCalendarDisplay();

            _previousMonthButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.GoToPreviousMonth());

            _nextMonthButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.GoToNextMonth());
        }

        private void UpdateCalendarDisplay()
        {
            var now = DateTime.Now;
            _nextMonthButton.interactable = _currentMonth != now.Month || _currentYear != now.Year;

            var (monthData, isInMonth) =
                _dateProgressService.GetMonthWeeks(_currentYear, _currentMonth);

            _currentMonthText.text =
                _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat.GetMonthName(_currentMonth);

            for (var day = 0; day < DaysInCalendar; day++)
            {
                var dailyProgress = monthData[day];
                var dayText = dailyProgress.Date.Day.ToString();
                var isOutsideMonth = isInMonth[day] is false;

                _calendarProgress[day].Init(dailyProgress.ProgressByState, dayText, isOutsideMonth);
            }
        }

        private void GoToPreviousMonth()
        {
            _currentMonth--;
            if (_currentMonth < 1)
            {
                _currentMonth = MonthsInYear;
                _currentYear--;
            }

            UpdateCalendarDisplay();
        }

        private void GoToNextMonth()
        {
            _currentMonth++;
            if (_currentMonth > MonthsInYear)
            {
                _currentMonth = 1;
                _currentYear++;
            }

            UpdateCalendarDisplay();
        }
    }
}