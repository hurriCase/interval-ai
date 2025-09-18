using System.Collections.Generic;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.UI.Components.Animation;
using Source.Scripts.UI.Components.Animation.Base;
using Source.Scripts.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components.Accordion
{
    internal sealed class AccordionComponent : RectTransformBehaviour
    {
        [field: SerializeField] internal ButtonComponent ExpandButton { get; private set; }
        [field: SerializeField] internal RectTransform HiddenContentContainer { get; private set; }

        [SerializeField] private VisibilityState _initiallyState;

        [SerializeReferenceDropdown, SerializeReference]
        private List<IAnimationComponent<VisibilityState>> _animations;

        [SerializeField] private List<ScaleAnimation<VisibilityState>> _scaleAnimations;

        private VisibilityState _currentState;

        private void Awake()
        {
            _currentState = _initiallyState;

            ExpandButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.SwitchVisibility());

            PlayAnimation(_initiallyState, true);
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
                if (isInstant is false)
                    tween.OnUpdate(this, (self, _) => LayoutRebuilder.MarkLayoutForRebuild(self.RectTransform));
            }

            foreach (var animationComponent in _animations)
                animationComponent.PlayAnimation(visibilityState, isInstant);
        }
    }
}