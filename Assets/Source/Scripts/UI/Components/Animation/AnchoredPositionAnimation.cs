using System;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class AnchoredPositionAnimation<TState> : AnimationBase<TState, Vector2>
        where TState : unmanaged, Enum
    {
        [SerializeField] private RectTransform _target;

        protected override void SetValueInstant(Vector2 value)
        {
            _target.anchoredPosition = value;
        }

        protected override Tween CreateTween(AnimationData<Vector2> animationData)
            => Tween.UIAnchoredPosition(_target, animationData.Value, animationData.TweenSettings);
    }
}