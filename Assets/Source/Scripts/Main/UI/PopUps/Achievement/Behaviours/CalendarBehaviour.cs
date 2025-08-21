using System;
using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours
{
    internal sealed class CalendarBehaviour : MonoBehaviour
    {
        [SerializeField] private WeekDaysBehaviour _weekDaysBehaviour;
        [SerializeField] private TextMeshProUGUI _currentMonthText;
        [SerializeField] private ButtonComponent _previousMonthButton;
        [SerializeField] private ButtonComponent _nextMonthButton;
        [SerializeField] private WeekProgressContainer[] _weekProgressContainers
            = new WeekProgressContainer[MaxWeeksInMonth];

        [Inject] private IDateProgressService _dateProgressService;
        [Inject] private ISettingsRepository _settingsRepository;

        private const int MaxWeeksInMonth = 6;
        private const int MonthsInYear = 12;

        private int _currentYear;
        private int _currentMonth;

        internal void Init()
        {
            _weekDaysBehaviour.Init();

            var now = DateTime.Now;
            _currentYear = now.Year;
            _currentMonth = now.Month;

            UpdateCalendarDisplay();

            _previousMonthButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.GoToPreviousMonth());

            _nextMonthButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.GoToNextMonth());
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

        private void UpdateCalendarDisplay()
        {
            var (monthData, isInMonth) = _dateProgressService.GetMonthWeeks(_currentYear, _currentMonth);
            _currentMonthText.text =
                _settingsRepository.CurrentCulture.Value.DateTimeFormat.GetMonthName(_currentMonth);

            for (var week = 0; week < MaxWeeksInMonth; week++)
                _weekProgressContainers[week].UpdateMonthWeeklyProgress(monthData, week, isInMonth);
        }
    }
}