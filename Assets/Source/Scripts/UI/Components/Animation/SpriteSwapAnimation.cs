﻿using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using PrimeTween;
using Source.Scripts.UI.Components.Animation.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components.Animation
{
    [Serializable]
    internal sealed class SpriteSwapAnimation<TState> : IAnimationComponent<TState>
        where TState : unmanaged, Enum
    {
        [SerializeField] private Image _target;
        [SerializeField] private float _duration;
        [SerializeField] private EnumArray<TState, Sprite> _states;

        private Tween _currentAnimation;

        private Sprite _targetSprite;

        public Tween PlayAnimation(TState state, bool isInstant)
        {
            _targetSprite = _states[state];

            if (isInstant)
            {
                UpdateSprite();
                return default;
            }

            if (_currentAnimation.isAlive)
                _currentAnimation.Stop();

            return _currentAnimation = Tween.Delay(this, _duration, self => self.UpdateSprite());
        }

        private void UpdateSprite()
        {
            _target.sprite = _targetSprite;
        }

        public void CancelAnimation()
        {
            _currentAnimation.Stop();
        }
    }
}