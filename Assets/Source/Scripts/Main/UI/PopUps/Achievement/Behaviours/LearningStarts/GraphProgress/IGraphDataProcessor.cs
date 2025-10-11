using Source.Scripts.UI.Behaviours.DateLabel.Range;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal interface IGraphDataProcessor
    {
        GraphDisplayData GetDisplayGraphData(DateRange dateRange, int pointsCount);
    }
}