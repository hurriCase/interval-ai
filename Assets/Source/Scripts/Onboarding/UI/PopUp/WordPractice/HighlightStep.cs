using System;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.Scripts.Onboarding.UI.PopUp.WordPractice
{
    [Serializable]
    internal sealed class HighlightStep : PracticeStepBase
    {
        [SerializeField] private ButtonComponent _switchButton;

        private GameObject _placeholderObject;
        // private RectTransform _placeholderRect;
        // private RectTransform _switchButtonRect;
        private Transform _previousParent;
        private int _siblingIndex;
        private Transform _tintParent;

        private IDisposable _subscription;

        internal override void Init(Transform tintParent, CancellationToken cancellationToken)
        {
            _tintParent = tintParent;

            _subscription = _switchButton.OnPointerClickAsObservable()
                .Subscribe(buttonClickSubject, static (_, clickSubject) => clickSubject.OnNext(Unit.Default));

            _subscription.RegisterTo(cancellationToken);

            _previousParent = _switchButton.transform.parent;
            _siblingIndex = _switchButton.transform.GetSiblingIndex();
        }

        internal override void ActiveStep()
        {
            _tintParent.SetActive(true);

            ApplyHighlightEffect();
        }

        internal override void HideStep()
        {
            RestoreButton();

            _subscription.Dispose();
        }

        private void ApplyHighlightEffect()
        {
            CreatePlaceholder();

            _switchButton.transform.SetParent(_tintParent);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_placeholderObject.transform.parent as RectTransform);
        }

        private void CreatePlaceholder()
        {
            _placeholderObject = Object.Instantiate(_switchButton.gameObject, _previousParent);

            _placeholderObject.transform.SetSiblingIndex(_siblingIndex);
            var placeholderCanvas = _placeholderObject.AddComponent<CanvasGroup>();
            placeholderCanvas.alpha = 0f;

            // _placeholderRect = _placeholderObject.GetComponent<RectTransform>();
            // _switchButtonRect = _switchButton.GetComponent<RectTransform>();
            // _placeholderRect.OnRectTransformDimensionsChangeAsObservable()
            //     .Subscribe(this, static (_, self) => self.UpdateButtonToMatchPlaceholder())
            //     .RegisterTo(_placeholderObject.GetCancellationTokenOnDestroy());
        }

        // private void UpdateButtonToMatchPlaceholder()
        // {
        //     var position = _placeholderRect.position;
        //     var pivot = _placeholderRect.pivot;
        //     var rect = _placeholderRect.rect;
        //     var xPosition = position.x + pivot.x * rect.width;
        //     var yPosition = position.y - pivot.y * rect.height;
        //     var targetPosition = new Vector3(xPosition, yPosition);
        //
        //     _switchButtonRect.position = targetPosition;
        //     _switchButtonRect.sizeDelta = _placeholderRect.sizeDelta;
        // }

        private void RestoreButton()
        {
            Object.Destroy(_placeholderObject);

            _switchButton.transform.SetParent(_previousParent);
            _switchButton.transform.SetSiblingIndex(_siblingIndex);
        }
    }
}