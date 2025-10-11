using Source.Scripts.UI.Components.DateLabel;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal interface IGraphDataProcessor
    {
        GraphDisplayData GetDisplayGraphData(DateRange dateRange, int pointsCount);
    }
}