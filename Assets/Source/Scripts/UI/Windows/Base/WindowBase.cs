using System;
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

        internal event Action OnHideWindow;

        private void OnValidate()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        internal void Show()
        {
            Tween.Alpha(_canvasGroup, 1f, _animationDuration);
        }

        internal void Hide()
        {
            Tween.Alpha(_canvasGroup, 0f, _animationDuration)
                .OnComplete(this, ui => ui.OnHideWindow?.Invoke());
        }
    }
}