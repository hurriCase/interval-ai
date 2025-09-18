using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.Core.Configs
{
    internal sealed class SwipeConfig : ScriptableObject, ISwipeConfig
    {
        [field: SerializeField] public float ReturnDuration { get; private set; }
        [field: SerializeField] public float SwipeExecuteDuration { get; private set; }
        [field: SerializeField] public float MaxDistanceRatio { get; private set; }
        [field: SerializeField] public float PickupScale { get; private set; }
        [field: SerializeField] public float PickupDuration { get; private set; }
        [field: SerializeField] public float HorizontalDragThresholdRatio { get; private set; }
        [field: SerializeField] public float VerticalToleranceRatio { get; private set; }

        [SerializeField] private AnimationCurve _liftHeightRatio;
        [SerializeField] private AnimationCurve _rotationDegrees;

        public float EvaluateLiftHeightRatio(float dragIntensity) => _liftHeightRatio.Evaluate(dragIntensity);
        public float EvaluateRotationDegrees(float dragIntensity) => _rotationDegrees.Evaluate(dragIntensity);
        public float GetMaxRotationDegrees() => _rotationDegrees.GetLastValue();
    }
}