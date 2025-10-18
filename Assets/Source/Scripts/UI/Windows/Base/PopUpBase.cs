using CustomUtils.Runtime.Animations.Base;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Data;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class PopUpBase : WindowBase
    {
        [field: SerializeField] internal bool IsSingle { get; private set; } = true;

        [SerializeReference, SerializeReferenceDropdown] private IAnimation<VisibilityState> _visibilityAnimation;
        [SerializeField] private ThemeButton _closeButton;

        [Inject] protected IAnimationsConfig animationsConfig;

        internal Observable<Unit> OnPopUpHidden => _popUpHidden;
        private readonly Subject<Unit> _popUpHidden = new();

        internal override void BaseInit()
        {
            _closeButton.AsNullable()?.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.HideAsync().Forget());
        }

        internal override async UniTask ShowAsync() => await _visibilityAnimation.PlayAnimation(VisibilityState.Visible);

        internal override async UniTask HideAsync()
        {
            await _visibilityAnimation.PlayAnimation(VisibilityState.Hidden);

            _popUpHidden.OnNext(Unit.Default);
        }

        internal override void HideImmediately()
        {
            _visibilityAnimation.PlayAnimation(VisibilityState.Hidden, true);

            _popUpHidden.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            _popUpHidden?.Dispose();
        }
    }
}