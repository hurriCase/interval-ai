using System.Collections.Generic;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using R3.Triggers;
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
        [SerializeField] private float _expandButtonAnimationDuration;
        [SerializeField] private float _hiddenElementsAnimationDuration;
        [SerializeField] private float _expandedRotationZ;
        [SerializeField] private float _collapsedRotationZ;

        private bool _isExpanded;
        private Sequence _currentAnimation;
        private float _currentRotationZ;

        internal void Init()
        {
            ExpandButton.OnClickAsObservable()
                .Subscribe(this,
                    static (_, component) => component.SwitchContent(component._isExpanded is false))
                .RegisterTo(destroyCancellationToken);

            SwitchContent(_isInitiallyExpanded, true);

            ShownContent.OnRectTransformDimensionsChangeAsObservable()
                .Subscribe(this, static (_, component) => component.UpdateContainerHeight())
                .RegisterTo(destroyCancellationToken);
        }

        private void SwitchContent(bool isExpanded, bool isInstant = false)
        {
            _isExpanded = isExpanded;

            if (_currentAnimation.isAlive)
                _currentAnimation.Stop();

            _currentAnimation = Sequence.Create()
                .Chain(RotateExpandButtonTween(isInstant ? 0f : _expandButtonAnimationDuration))
                .Group(AnimateHiddenContentTweens(isExpanded, isInstant ? 0f : _hiddenElementsAnimationDuration));
        }

        private Tween RotateExpandButtonTween(float duration)
        {
            var targetZ = _isExpanded ? _expandedRotationZ : _collapsedRotationZ;

            var deltaZ = Mathf.DeltaAngle(_currentRotationZ, targetZ);
            var finalTargetZ = _currentRotationZ + deltaZ;

            return Tween.Custom(this,
                _currentRotationZ,
                finalTargetZ,
                duration,
                (component, rotationZ) => component.SetButtonRotation(rotationZ));
        }

        private void SetButtonRotation(float rotationZ)
        {
            var buttonTransform = ExpandButton.transform;
            var euler = buttonTransform.eulerAngles;
            euler.z = rotationZ;
            buttonTransform.eulerAngles = euler;

            _currentRotationZ = rotationZ;
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

        private void UpdateContainerHeight()
        {
            var hiddenRectTransform = HiddenContentContainer.RectTransform;
            var hiddenContentSize = hiddenRectTransform.rect.height * hiddenRectTransform.localScale.y;
            var totalHeight = ShownContent.rect.height + hiddenContentSize;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, totalHeight);

            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform.parent as RectTransform);
        }
    }
}