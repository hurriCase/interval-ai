using CustomUtils.Runtime.Animations.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal sealed class PopUpVisibilityHandler : MonoBehaviour
    {
        [SerializeReference, SerializeReferenceDropdown] private IAnimation<VisibilityState> _visibilityAnimation;

        internal async UniTask ShowAsync() => await _visibilityAnimation.PlayAnimation(VisibilityState.Visible);
        internal async UniTask HideAsync() => await _visibilityAnimation.PlayAnimation(VisibilityState.Hidden);
        internal void HideImmediately() => _visibilityAnimation.PlayAnimation(VisibilityState.Hidden, true);
    }
}