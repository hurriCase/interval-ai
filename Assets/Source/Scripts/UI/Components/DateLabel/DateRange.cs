using System;
using System.Globalization;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using UnityEngine;

namespace Source.Scripts.UI.Components.DateLabel
{
    [Serializable]
    internal struct DateRange
    {
        [field: SerializeField] internal DateType DateType { get; private set; }
        [field: SerializeField] internal int Amount { get; private set; }

        private const int DaysInWeek = 7;

        internal int GetDayCount()
        {
            return DateType switch
            {
                DateType.Days => Amount,
                DateType.Weeks => Amount * DaysInWeek,
                DateType.Months => CalculateTotalDaysInMonths(),
                DateType.Years => CalculateTotalDaysInYears(),
                _ => 0
            };
        }

        private int CalculateTotalDaysInMonths()
        {
            var currentDate = DateTime.Now.Date;
            var totalDays = 0;

            for (var i = 0; i < Amount; i++)
            {
                totalDays += DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                currentDate = currentDate.AddMonths(-1);
            }

            return totalDays;
        }

        private int CalculateTotalDaysInYears()
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var currentDate = DateTime.Now.Date;
            var totalDays = 0;

            for (var i = 0; i < Amount; i++)
            {
                totalDays += calendar.GetDaysInYear(currentDate.Year);
                currentDate = currentDate.AddYears(-1);
            }

            return totalDays;
        }
    }
}