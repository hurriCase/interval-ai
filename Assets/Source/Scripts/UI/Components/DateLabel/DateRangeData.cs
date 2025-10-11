using System;

namespace Source.Scripts.UI.Components.DateLabel
{
    internal readonly struct DateRangeData
    {
        internal DateTime StartDate { get; }
        internal DateTime EndDate { get; }
        internal int TotalDays { get; }
        internal DateTime[] DatePoints { get; }

        internal DateRangeData(DateTime startDate, DateTime endDate, int totalDays, DateTime[] datePoints)
        {
            StartDate = startDate;
            EndDate = endDate;
            TotalDays = totalDays;
            DatePoints = datePoints;
        }
    }
}