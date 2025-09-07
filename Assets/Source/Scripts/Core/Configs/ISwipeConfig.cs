namespace Source.Scripts.Core.Configs
{
    internal interface ISwipeConfig
    {
        float ReturnDuration { get; }
        float SwipeExecuteDuration { get; }
        float MaxDistanceRatio { get; }
        float PickupScale { get; }
        float PickupDuration { get; }
        float HorizontalDragThresholdRatio { get; }
        float VerticalToleranceRatio { get; }
        float EvaluateLiftHeightRatio(float dragIntensity);
        float EvaluateRotationDegrees(float dragIntensity);
        float GetMaxRotationDegrees();
    }
}