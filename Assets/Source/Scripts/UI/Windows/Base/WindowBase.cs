using System;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    [Serializable]
    [RequireComponent(typeof(CanvasGroup))]
    internal abstract class WindowBase<T> : MonoBehaviour where T : Enum
    {
        [field: SerializeField] internal T UIType { get; private set; }

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _animationDuration = 0.1f;

        private void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        internal virtual void BaseInit() { }

        internal virtual void Init() { }

        internal virtual void Show()
        {
            Tween.Alpha(_canvasGroup, 1f, _animationDuration)
                .OnComplete(this, ui => ui._canvasGroup.Show());
        }

        internal virtual void Hide()
        {
            Tween.Alpha(_canvasGroup, 0f, _animationDuration)
                .OnComplete(HideImmediately);
        }

        internal virtual void HideImmediately() => _canvasGroup.Hide();
    }
}