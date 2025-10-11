using System.Collections.Generic;
using Source.Scripts.UI.Behaviours.DateLabel.Range;

namespace Source.Scripts.Main.Data.Base
{
    internal interface IProgressGraphSettings
    {
        List<DateRange> GraphProgressRanges { get; }
        int GraphPointsCount { get; }
    }
}