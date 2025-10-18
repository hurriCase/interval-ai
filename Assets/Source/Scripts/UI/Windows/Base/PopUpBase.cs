using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class PopUpBase : WindowBase
    {
        [field: SerializeField] internal bool IsSingle { get; private set; } = true;

        [SerializeField] private PopUpVisibilityHandler _popUpVisibilityHandler;

        [SerializeField] private ThemeButton _closeButton;

        internal Observable<Unit> OnPopUpHidden => _popUpHidden;
        private readonly Subject<Unit> _popUpHidden = new();

        internal override void BaseInit()
        {
            _closeButton.AsNullable()?.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.HideAsync().Forget());
        }

        internal override async UniTask ShowAsync()
        {
            await _popUpVisibilityHandler.ShowAsync();
        }

        internal override async UniTask HideAsync()
        {
            await _popUpVisibilityHandler.HideAsync();

            _popUpHidden.OnNext(Unit.Default);
        }

        internal override void HideImmediately()
        {
            _popUpVisibilityHandler.HideImmediately();

            _popUpHidden.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            _popUpHidden?.Dispose();
        }
    }
}