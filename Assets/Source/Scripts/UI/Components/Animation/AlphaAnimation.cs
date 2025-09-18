using System;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class AlphaAnimation<TState> : AnimationBase<TState, float>
        where TState : unmanaged, Enum
    {
        [SerializeField] private CanvasGroup _target;

        protected override void SetValueInstant(float value)
        {
            _target.alpha = value;
        }

        protected override Tween CreateTween(AnimationData<float> animationData)
            => Tween.Alpha(_target, animationData.Value, animationData.TweenSettings);
    }
}