namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal interface IGraphDataProcessor
    {
        GraphDisplayData GetDisplayGraphData(int totalDays, int pointsCount);
    }
}