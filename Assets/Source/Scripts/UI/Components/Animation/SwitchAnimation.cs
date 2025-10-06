using System;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class SwitchAnimation<TState> : AnimationBase<TState, CanvasGroup>
        where TState : unmanaged, Enum
    {
        private CanvasGroup _currentCanvasGroup;

        protected override void SetValueInstant(CanvasGroup canvasGroup)
        {
            _currentCanvasGroup = canvasGroup;

            HideOther();
            ShowCurrent();
        }

        protected override Tween CreateTween(AnimationData<CanvasGroup> animationData)
        {
            _currentCanvasGroup = animationData.Value;

            HideOther();

            return Tween.Alpha(animationData.Value, 1f, animationData.TweenSettings)
                .OnComplete(this, self => self.ShowCurrent());
        }

        private void HideOther()
        {
            foreach (var state in states)
            {
                state.Value.AsNullable()?.Hide();
                state.Value.AsNullable()?.SetActive(false);
            }
        }

        private void ShowCurrent()
        {
            _currentCanvasGroup.Show();
            _currentCanvasGroup.SetActive(true);
        }
    }
}