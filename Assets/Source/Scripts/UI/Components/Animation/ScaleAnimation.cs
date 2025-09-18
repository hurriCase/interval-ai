using System;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class ScaleAnimation<TState> : AnimationBase<TState, Vector3>
        where TState : unmanaged, Enum
    {
        [SerializeField] private Transform _target;

        protected override void SetValueInstant(Vector3 value)
        {
            _target.localScale = value;
        }

        protected override Tween CreateTween(AnimationData<Vector3> animationData)
            => Tween.Scale(_target, animationData.Value, animationData.TweenSettings);
    }
}