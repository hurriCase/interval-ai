using System.Collections.Generic;
using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.Animations.Base;
using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using UnityEngine;

namespace Source.Scripts.UI.Components.Accordion
{
    internal sealed class AccordionComponent : MonoBehaviour
    {
        [field: SerializeField] internal ThemeButton ExpandButton { get; private set; }
        [field: SerializeField] internal RectTransform HiddenContentContainer { get; private set; }

        [SerializeField] private VisibilityState _initiallyState;
        [SerializeField] private bool _isInitiallyReady = true;

        [SerializeReferenceDropdown, SerializeReference]
        private List<IAnimation<VisibilityState>> _animations;

        [SerializeField] private List<ScaleAnimation<VisibilityState>> _scaleAnimations;

        [SerializeField, Self] private RectTransform _rectTransform;

        private VisibilityState _currentState;

        private void Awake()
        {
            _currentState = _initiallyState;

            ExpandButton.OnClickAsObservable()
                .Where(this, static self => self._isInitiallyReady)
                .SubscribeUntilDestroy(this, static self => self.SwitchVisibility());

            PlayAnimation(_initiallyState, true);
        }

        internal void SetReady(VisibilityState visibilityState, bool isInstant = false)
        {
            _isInitiallyReady = true;
            PlayAnimation(visibilityState, isInstant);
        }

        private void SwitchVisibility()
        {
            var newState = _currentState == VisibilityState.Hidden ? VisibilityState.Visible : VisibilityState.Hidden;
            PlayAnimation(newState);
            _currentState = newState;
        }

        private void PlayAnimation(VisibilityState visibilityState, bool isInstant = false)
        {
            foreach (var animationComponent in _scaleAnimations)
            {
                var tween = animationComponent.PlayAnimation(visibilityState, isInstant);
                if (isInstant)
                    continue;

                tween.OnUpdate(this, static (self, _) => self._rectTransform.MarkLayoutForRebuild());
            }

            foreach (var animationComponent in _animations)
                animationComponent.PlayAnimation(visibilityState, isInstant);
        }
    }
}