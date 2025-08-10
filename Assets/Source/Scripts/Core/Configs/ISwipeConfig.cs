namespace Source.Scripts.Core.Configs
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