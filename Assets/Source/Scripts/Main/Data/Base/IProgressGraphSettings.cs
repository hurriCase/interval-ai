using System.Collections.Generic;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;

namespace Source.Scripts.Main.Data.Base
{
    internal interface IProgressGraphSettings
    {
        List<DateRange> GraphProgressRanges { get; }
        int GraphPointsCount { get; }
    }
}