using System;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.UI.Components.DateLabel
{
    internal sealed class DateRangeCalculator : IDateRangeCalculator
    {
        private const int DaysInWeek = 7;

        private readonly IUISettingsRepository _uiSettingsRepository;

        internal DateRangeCalculator(IUISettingsRepository uiSettingsRepository)
        {
            _uiSettingsRepository = uiSettingsRepository;
        }

        public DateRangeData Calculate(DateRange dateRange, int pointsCount)
        {
            var endDate = DateTime.Now.Date;
            var totalDays = CalculateTotalDays(dateRange);
            var startDate = endDate.AddDays(-totalDays + 1);
            var datePoints = CalculateDatePoints(startDate, endDate, pointsCount);

            return new DateRangeData(startDate, endDate, totalDays, datePoints);
        }

        private int CalculateTotalDays(DateRange dateRange)
        {
            return dateRange.DateType switch
            {
                DateType.Days => dateRange.Amount,
                DateType.Weeks => dateRange.Amount * DaysInWeek,
                DateType.Months => CalculateTotalDaysInMonths(dateRange.Amount),
                DateType.Years => CalculateTotalDaysInYears(dateRange.Amount),
                _ => 0
            };
        }

        private DateTime[] CalculateDatePoints(DateTime startDate, DateTime endDate, int pointsCount)
        {
            if (pointsCount <= 0)
                return Array.Empty<DateTime>();

            var totalDays = (endDate - startDate).Days;
            var datePoints = new DateTime[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var progress = pointsCount > 1 ? (float)i / (pointsCount - 1) : 0f;
                var daysToAdd = (int)Math.Round(totalDays * progress);
                datePoints[i] = startDate.AddDays(daysToAdd);
            }

            return datePoints;
        }

        private int CalculateTotalDaysInMonths(int monthsCount)
        {
            var currentDate = DateTime.Now.Date;
            var totalDays = 0;

            for (var i = 0; i < monthsCount; i++)
            {
                totalDays += DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                currentDate = currentDate.AddMonths(-1);
            }

            return totalDays;
        }

        private int CalculateTotalDaysInYears(int yearsCount)
        {
            var calendar = _uiSettingsRepository.CurrentCulture.Value.Calendar;
            var currentDate = DateTime.Now.Date;
            var totalDays = 0;

            for (var i = 0; i < yearsCount; i++)
            {
                totalDays += calendar.GetDaysInYear(currentDate.Year);
                currentDate = currentDate.AddYears(-1);
            }

            return totalDays;
        }
    }
}