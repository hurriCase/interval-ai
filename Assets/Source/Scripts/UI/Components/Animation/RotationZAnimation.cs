using System;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class RotationZAnimation<TState> : AnimationBase<TState, float>
        where TState : unmanaged, Enum
    {
        [SerializeField] private RectTransform _target;

        private float _currentRotationZ;

        protected override void SetValueInstant(float value)
        {
            var endValue = CalculateFinalZ(value);
            SetRotation(endValue);
        }

        protected override Tween CreateTween(AnimationData<float> animationData)
            => Tween.Custom(this,
                _currentRotationZ,
                CalculateFinalZ(animationData.Value),
                animationData.TweenSettings,
                (self, rotationZ) => self.SetRotation(rotationZ));

        private void SetRotation(float rotationZ)
        {
            var targetTransform = _target.transform;
            var euler = targetTransform.eulerAngles;
            euler.z = rotationZ;
            targetTransform.eulerAngles = euler;

            _currentRotationZ = rotationZ;
        }

        private float CalculateFinalZ(float targetZ)
        {
            var deltaZ = Mathf.DeltaAngle(_currentRotationZ, targetZ);
            return _currentRotationZ + deltaZ;
        }
    }
}