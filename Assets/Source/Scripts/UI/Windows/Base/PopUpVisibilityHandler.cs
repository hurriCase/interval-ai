using CustomUtils.Runtime.Animations.Base;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal class PopUpVisibilityHandler : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeReference, SerializeReferenceDropdown] private IAnimation<VisibilityState> _visibilityAnimation;

        internal virtual async UniTask ShowAsync()
        {
            _canvasGroup.Show();

            await _visibilityAnimation.PlayAnimation(VisibilityState.Visible);
        }

        internal virtual async UniTask HideAsync()
        {
            await _visibilityAnimation.PlayAnimation(VisibilityState.Hidden);

            _canvasGroup.Hide();
        }

        internal void HideImmediately()
        {
            _visibilityAnimation.PlayAnimation(VisibilityState.Hidden, true);

            _canvasGroup.Hide();
        }
    }
}