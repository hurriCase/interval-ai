using System;
using R3;
using Source.Scripts.Data.Repositories.Progress.Date;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.UI.CustomButton;
using Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts;
using Source.Scripts.UI.Windows.Shared;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement
{
    internal sealed class CalendarBehaviour : MonoBehaviour
    {
        [SerializeField] private WeekDaysBehaviour _weekDaysBehaviour;
        [SerializeField] private TextMeshProUGUI _currentMonthText;
        [SerializeField] private ButtonComponent _previousMonthButton;
        [SerializeField] private ButtonComponent _nextMonthButton;
        [SerializeField] private WeekProgressContainer[] _weekProgressContainers = new WeekProgressContainer[6];

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
                .Subscribe(this, static (_, behaviour) => behaviour.GoToPreviousMonth());

            _nextMonthButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) => behaviour.GoToNextMonth());
        }

        private void GoToPreviousMonth()
        {
            _currentMonth--;
            if (_currentMonth < 1)
            {
                _currentMonth = 12;
                _currentYear--;
            }

            UpdateCalendarDisplay();
        }

        private void GoToNextMonth()
        {
            _currentMonth++;
            if (_currentMonth > 12)
            {
                _currentMonth = 1;
                _currentYear++;
            }

            UpdateCalendarDisplay();
        }

        private void UpdateCalendarDisplay()
        {
            var (monthData, isInMonth) = WeekProgressHelper.GetMonthWeeks(_currentYear, _currentMonth);
            _currentMonthText.text = UserRepository.Instance.CurrentCulture.DateTimeFormat.GetMonthName(_currentMonth);

            for (var week = 0; week < 6; week++)
                _weekProgressContainers[week].UpdateMonthWeeklyProgress(monthData, week, isInMonth);
        }
    }
}