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
        [SerializeField] private AnimationAxis _axis = AnimationAxis.Both;

        protected override void SetValueInstant(Vector2 value)
        {
            if (_axis == AnimationAxis.None)
                return;

            _target.anchoredPosition = _axis switch
            {
                AnimationAxis.X => new Vector2(value.x, _target.anchoredPosition.y),
                AnimationAxis.Y => new Vector2(_target.anchoredPosition.x, value.y),
                _ => value
            };
        }

        protected override Tween CreateTween(AnimationData<Vector2> animationData)
        {
            if (_axis == AnimationAxis.None)
                return Tween.Delay(0f);

            var endValue = animationData.Value;
            return _axis switch
            {
                AnimationAxis.X => Tween.UIAnchoredPositionX(_target, endValue.x, animationData.TweenSettings),
                AnimationAxis.Y => Tween.UIAnchoredPositionY(_target, endValue.y, animationData.TweenSettings),
                _ => Tween.UIAnchoredPosition(_target, endValue, animationData.TweenSettings)
            };
        }
    }
}