using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class AccordionComponent : MonoBehaviour
    {
        [field: SerializeField] internal ButtonComponent ExpandButton { get; private set; }
        [field: SerializeField] internal List<CanvasGroup> HiddenContent { get; private set; }

        [SerializeField] private bool _isInitiallyExpanded;
        [SerializeField] private float _expandButtonAnimationDuration;
        [SerializeField] private float _hiddenElementsAnimationDuration;

        private bool _isExpanded;
        private Sequence _currentAnimation;

        private void Awake()
        {
            ExpandButton.OnClickAsObservable()
                .Subscribe(this,
                    static (_, component) => component.SwitchContent(component._isExpanded is false))
                .RegisterTo(destroyCancellationToken);

            SwitchContent(_isInitiallyExpanded);
        }

        private void SwitchContent(bool isExpanded)
        {
            _isExpanded = isExpanded;

            if (_currentAnimation.isAlive)
                _currentAnimation.Stop();

            _currentAnimation = Sequence.Create()
                .Chain(RotateExpandButtonTween())
                .Group(AnimateHiddenContentTweens(isExpanded));
        }

        private Tween RotateExpandButtonTween()
        {
            var currentAngles = ExpandButton.transform.eulerAngles;
            var targetZ = currentAngles.z + 180f;

            return Tween.Custom(this,
                currentAngles.z,
                targetZ,
                _expandButtonAnimationDuration,
                (component, rotationZ) =>
                {
                    var buttonTransform = component.ExpandButton.transform;
                    var euler = buttonTransform.eulerAngles;
                    euler.z = rotationZ;
                    buttonTransform.eulerAngles = euler;
                });
        }

        private Sequence AnimateHiddenContentTweens(bool isExpanded)
        {
            var contentSequence = Sequence.Create();

            foreach (var content in HiddenContent)
            {
                if (isExpanded)
                {
                    content.gameObject.SetActive(true);
                    contentSequence
                        .Group(Tween.ScaleY(content.transform, 1f, _hiddenElementsAnimationDuration))
                        .Group(Tween.Alpha(content, 1f, _hiddenElementsAnimationDuration));

                    continue;
                }

                contentSequence
                    .Group(Tween.ScaleY(content.transform, 0f, _hiddenElementsAnimationDuration))
                    .Group(Tween.Alpha(content, 0f, _hiddenElementsAnimationDuration));
            }

            if (isExpanded is false)
                contentSequence.OnComplete(HiddenContent, canvasGroups =>
                {
                    foreach (var canvasGroup in canvasGroups)
                        canvasGroup.SetActive(false);
                });

            return contentSequence;
        }
    }
}