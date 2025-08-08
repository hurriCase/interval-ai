using System;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.Scripts.Onboarding.UI.PopUp.WordPractice
{
    [Serializable]
    internal sealed class WordPracticeStepData
    {
        [field: SerializeField] internal string LocalizationKey { get; private set; }
        [field: SerializeField] internal ButtonComponent SwitchButton { get; private set; }

        private GameObject _placeholderObject;
        private Transform _previousParent;
        private int _siblingIndex;
        private Transform _tintParent;

        internal void Init(Transform tintParent)
        {
            _tintParent = tintParent;

            _previousParent = SwitchButton.transform.parent;
            _siblingIndex = SwitchButton.transform.GetSiblingIndex();
        }

        internal void ApplyHighlightEffect()
        {
            CreatePlaceholder();

            SwitchButton.transform.SetParent(_tintParent);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_placeholderObject.transform.parent as RectTransform);
        }

        private void CreatePlaceholder()
        {
            _placeholderObject = Object.Instantiate(SwitchButton.gameObject, _previousParent);
            _placeholderObject.transform.SetSiblingIndex(_siblingIndex);
            var placeholderCanvas = _placeholderObject.AddComponent<CanvasGroup>();
            placeholderCanvas.alpha = 0f;
        }

        internal void RestoreButton()
        {
            Object.Destroy(_placeholderObject);

            SwitchButton.transform.SetParent(_previousParent);
            SwitchButton.transform.SetSiblingIndex(_siblingIndex);
        }
    }
}