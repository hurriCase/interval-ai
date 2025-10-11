using System;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Behaviours.DateLabel.Base;

namespace Source.Scripts.UI.Behaviours.DateLabel.Range
{
    internal sealed class DateRangeCalculator : IDateRangeCalculator
    {
        private readonly IUISettingsRepository _uiSettingsRepository;

        internal DateRangeCalculator(IUISettingsRepository uiSettingsRepository)
        {
            _uiSettingsRepository = uiSettingsRepository;
        }

        public DateRangeData Calculate(DateRange dateRange, int pointsCount)
        {
            var endDate = DateTime.Now.Date;
            var totalDays = dateRange.CalculateDayCount(_uiSettingsRepository);
            var startDate = endDate.AddDays(-totalDays + 1);
            var datePoints = CalculateDatePoints(startDate, totalDays, pointsCount);

            return new DateRangeData(totalDays, datePoints);
        }

        private DateTime[] CalculateDatePoints(DateTime startDate, int totalDays, int pointsCount)
        {
            var datePoints = new DateTime[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var progress = pointsCount > 1 ? (float)i / (pointsCount - 1) : 0f;
                var daysToAdd = (int)Math.Round(totalDays * progress);
                datePoints[i] = startDate.AddDays(daysToAdd);
            }

            return datePoints;
        }
    }
}