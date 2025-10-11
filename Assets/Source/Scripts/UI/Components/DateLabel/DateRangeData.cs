using System;

namespace Source.Scripts.UI.Components.DateLabel
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