using System;
using Source.Scripts.UI.Behaviours.DateLabel.Base;

namespace Source.Scripts.UI.Behaviours.DateLabel.Range
{
    internal sealed class DateRangeCalculator : IDateRangeCalculator
    {
        public DateTime[] Calculate(int totalDays, int pointsCount)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-totalDays + 1);
            var datePoints = CalculateDatePoints(startDate, totalDays, pointsCount);

            return datePoints;
        }

        private DateTime[] CalculateDatePoints(DateTime startDate, int totalDays, int pointsCount)
        {
            if (pointsCount == 1)
                return new[] { startDate };

            var datePoints = new DateTime[pointsCount];
            var intervalBetweenPoints = totalDays / (pointsCount - 1f);

            for (var i = 0; i < pointsCount; i++)
            {
                var daysFromStart = intervalBetweenPoints * i;
                datePoints[i] = startDate.AddDays(daysFromStart);
            }

            return datePoints;
        }
    }
}