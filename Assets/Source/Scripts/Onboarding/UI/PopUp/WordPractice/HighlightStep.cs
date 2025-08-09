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
        }

        private void RestoreButton()
        {
            Object.Destroy(_placeholderObject);

            _switchButton.transform.SetParent(_previousParent);
            _switchButton.transform.SetSiblingIndex(_siblingIndex);
        }
    }
}