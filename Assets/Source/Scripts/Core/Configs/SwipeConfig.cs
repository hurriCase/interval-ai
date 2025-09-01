using UnityEngine;

namespace Source.Scripts.Core.Configs
{
    internal sealed class SwipeConfig : ScriptableObject, ISwipeConfig
    {
        [field: SerializeField] public float ReturnDuration { get; private set; }
        [field: SerializeField] public float SwipeExecuteDuration { get; private set; }
        [field: SerializeField] public float MaxRotationDegrees { get; private set; }
        [field: SerializeField] public float MaxLiftHeightRatio { get; private set; }
        [field: SerializeField] public float PickupScale { get; private set; }
        [field: SerializeField] public float PickupDuration { get; private set; }
        [field: SerializeField] public float HorizontalDragThresholdRatio { get; private set; }
        [field: SerializeField] public float VerticalToleranceRatio { get; private set; }
    }
}