using System.Collections.Generic;
using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Components
{
    internal sealed class AccordionComponent : RectTransformBehaviour
    {
        [field: SerializeField] internal ButtonComponent ExpandButton { get; private set; }
        [field: SerializeField] internal AccordionItem HiddenContentContainer { get; private set; }
        [field: SerializeField] internal RectTransform ShownContent { get; private set; }
        [field: SerializeField] internal List<AccordionItem> HiddenContent { get; private set; }

        [SerializeField] private bool _isInitiallyExpanded;

        [SerializeField] private float _hiddenElementsAnimationDuration;

        [SerializeField] private bool _useExpandButtonAnimation;

        [HideIf(nameof(_useExpandButtonAnimation))]
        [SerializeField] private float _expandButtonAnimationDuration;

        [HideIf(nameof(_useExpandButtonAnimation))]
        [SerializeField] private float _expandedRotationZ;

        [HideIf(nameof(_useExpandButtonAnimation))]
        [SerializeField] private float _collapsedRotationZ;

        private bool _isExpanded;
        private Sequence _currentAnimation;
        private float _currentRotationZ;

        internal void Init()
        {
            ExpandButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.SwitchContent(self._isExpanded is false));

            SwitchContent(_isInitiallyExpanded, true);
        }

        private void SwitchContent(bool isExpanded, bool isInstant = false)
        {
            _isExpanded = isExpanded;

            if (_currentAnimation.isAlive)
                _currentAnimation.Stop();

            var sequence = Sequence.Create();

            TryAddButtonAnimation(ref sequence, isInstant);

            _currentAnimation = sequence
                .Group(AnimateHiddenContentTweens(isExpanded, isInstant ? 0f : _hiddenElementsAnimationDuration));
        }

        private void TryAddButtonAnimation(ref Sequence sequence, bool isInstant)
        {
            if (_useExpandButtonAnimation is false)
                return;

            if (isInstant)
            {
                SetButtonRotation(CalculateFinalZ());
                return;
            }

            var buttonAnimation = Tween.Custom(this,
                _currentRotationZ,
                CalculateFinalZ(),
                _expandButtonAnimationDuration,
                (component, rotationZ) => component.SetButtonRotation(rotationZ));

            sequence.Chain(buttonAnimation);
        }

        private void SetHiddenContent(bool isExpanded, float endValue)
        {
            if (isExpanded)
                HiddenContentContainer.CanvasGroup.Show();

            var scale = HiddenContentContainer.RectTransform.localScale;
            scale.y = endValue;
            HiddenContentContainer.RectTransform.localScale = scale;

            HiddenContentContainer.CanvasGroup.alpha = endValue;
            UpdateContainerHeight();
        }

        private Sequence AnimateHiddenContentTweens(bool isExpanded, float duration) =>
            isExpanded
                ? CreateHiddenContentAnimation(1f, duration)
                    .OnComplete(HiddenContentContainer, accordionItem => accordionItem.CanvasGroup.Show())
                : CreateHiddenContentAnimation(0f, duration)
                    .OnComplete(HiddenContentContainer, accordionItem => accordionItem.CanvasGroup.Hide());

        private Sequence CreateHiddenContentAnimation(float endValue, float duration) =>
            Sequence.Create()
                .Group(Tween.ScaleY(HiddenContentContainer.RectTransform, endValue, duration))
                .Group(Tween.Alpha(HiddenContentContainer.CanvasGroup, endValue, duration))
                .Group(Tween.Custom(this, 0f, 1f, duration,
                    (component, _) => component.UpdateContainerHeight()));

        private void SetButtonRotation(float rotationZ)
        {
            var buttonTransform = ExpandButton.transform;
            var euler = buttonTransform.eulerAngles;
            euler.z = rotationZ;
            buttonTransform.eulerAngles = euler;

            _currentRotationZ = rotationZ;
        }

        private void UpdateContainerHeight()
        {
            var hiddenRectTransform = HiddenContentContainer.RectTransform;
            var hiddenContentSize = hiddenRectTransform.rect.height * hiddenRectTransform.localScale.y;
            var totalHeight = ShownContent.rect.height + hiddenContentSize;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, totalHeight);

            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform.parent as RectTransform);
        }

        private float CalculateFinalZ()
        {
            var targetZ = _isExpanded ? _expandedRotationZ : _collapsedRotationZ;

            var deltaZ = Mathf.DeltaAngle(_currentRotationZ, targetZ);
            return _currentRotationZ + deltaZ;
        }
    }
}