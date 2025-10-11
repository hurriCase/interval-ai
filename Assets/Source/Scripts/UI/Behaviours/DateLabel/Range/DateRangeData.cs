using System;

namespace Source.Scripts.UI.Behaviours.DateLabel.Range
{
    internal readonly struct DateRangeData
    {
        internal int TotalDays { get; }
        internal DateTime[] DatePoints { get; }

        internal DateRangeData(int totalDays, DateTime[] datePoints)
        {
            TotalDays = totalDays;
            DatePoints = datePoints;
        }
    }
}