namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Swipe
{
    internal interface ISwipeConfig
    {
        float ReturnDuration { get; }
        float SwipeExecuteDuration { get; }
        float MaxRotationDegrees { get; }
        float MaxLiftHeightRatio { get; }
        float PickupScale { get; }
        float PickupDuration { get; }
        float HorizontalDragThresholdRatio { get; }
        float VerticalToleranceRatio { get; }
    }
}