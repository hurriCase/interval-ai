using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class WindowsController<TScreenEnum, TPopUpEnum> : MonoBehaviour
        where TScreenEnum : unmanaged, Enum
        where TPopUpEnum : unmanaged, Enum
    {
        [SerializeField]
        private EnumArray<TScreenEnum, AssetReferenceT<GameObject>> _screenReferences = new(EnumMode.SkipFirst);

        [SerializeField]
        private EnumArray<TPopUpEnum, AssetReferenceT<GameObject>> _popUpReferences = new(EnumMode.SkipFirst);

        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popUpsContainer;

        public TScreenEnum InitialScreenType { get; private set; }
        public TPopUpEnum CurrentPopUpType { get; private set; }

        private EnumArray<TScreenEnum, ScreenBase> _createdScreens = new(EnumMode.SkipFirst);
        private EnumArray<TPopUpEnum, PopUpBase> _createdPopUps = new(EnumMode.SkipFirst);
        private readonly Stack<PopUpBase> _previousOpenedPopUps = new();

        private PopUpBase _currentOpenedPopUp;
        private ScreenBase _currentScreen;

        private TScreenEnum _initialScreenType;

        private IObjectResolver _objectResolver;
        private IAddressablesLoader _addressablesLoader;

        [Inject]
        public void Inject(IObjectResolver objectResolver, IAddressablesLoader addressablesLoader)
        {
            _objectResolver = objectResolver;
            _addressablesLoader = addressablesLoader;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var sourceWithDestroy = cancellationToken.CreateLinkedTokenSourceWithDestroy(this);

            await InitScreensAsync(sourceWithDestroy.Token);
            await InitPopUpAsync(sourceWithDestroy.Token);
        }

        private async UniTask InitScreensAsync(CancellationToken cancellationToken)
        {
            foreach (var (screenType, screenReference) in _screenReferences.AsTuples())
            {
                var loadedScreen = await _addressablesLoader.LoadAsync<GameObject>(screenReference, cancellationToken);
                var createdWindow = _objectResolver.Instantiate(loadedScreen, _screensContainer);

                if (createdWindow.TryGetComponent<ScreenBase>(out var screenBase) is false)
                    continue;

                _createdScreens[screenType] = screenBase;

                screenBase.BaseInit();
                screenBase.Init();

                if (screenBase.InitialWindow)
                {
                    InitialScreenType = screenType;
                    continue;
                }

                screenBase.HideImmediately();
            }
        }

        private async UniTask InitPopUpAsync(CancellationToken cancellationToken)
        {
            foreach (var (popUpType, popUpReference) in _popUpReferences.AsTuples())
            {
                var loadedPopUp = await _addressablesLoader.LoadAsync<GameObject>(popUpReference, cancellationToken);
                var createdWindow = _objectResolver.Instantiate(loadedPopUp, _popUpsContainer);

                if (createdWindow.TryGetComponent<PopUpBase>(out var popUpBase) is false)
                    continue;

                _createdPopUps[popUpType] = popUpBase;

                popUpBase.BaseInit();
                popUpBase.Init();
                popUpBase.HideImmediately();
                popUpBase.OnPopUpHidden.SubscribeUntilDestroy(this, static self => self.HandlePopUpHide());
            }
        }

        public void OpenScreenByType(TScreenEnum screenType)
        {
            HideAllPopUps();

            var screenBase = _createdScreens[screenType];

            if (!screenBase)
            {
                var message = ZString.Format("[WindowsController::OpenScreenByType] There is no screen with type: {0}",
                    screenType);
                Debug.LogError(message);
            }

            if (_currentScreen)
                _currentScreen.HideAsync();

            _currentScreen = screenBase;
            screenBase.ShowAsync();
        }

        public void OpenPopUpByType(TPopUpEnum popUpType)
        {
            var popUpBase = _createdPopUps[popUpType];

            if (!popUpBase)
            {
                var message = ZString.Format("[WindowsController::OpenPopUpByType] There is no pop up with type: {0}",
                    popUpType);
                Debug.LogError(message);
                return;
            }

            OpenPopUpAsync(popUpBase, popUpType).Forget();
        }

        public TPopUpType OpenPopUp<TPopUpType>() where TPopUpType : PopUpBase
        {
            foreach (var (popUpEnum, popUpBase) in _createdPopUps.AsTuples())
            {
                if (popUpBase.GetType() != typeof(TPopUpType))
                    continue;

                OpenPopUpAsync(popUpBase, popUpEnum).Forget();
                return popUpBase as TPopUpType;
            }

            Debug.LogError(ZString.Format("[WindowsController::OpenPopUp] There is no pop up with type: {0}",
                typeof(TPopUpType)));

            return null;
        }

        private async UniTask OpenPopUpAsync(PopUpBase popUpBase, TPopUpEnum popUpEnum)
        {
            if (_currentOpenedPopUp && popUpBase.IsInFrontOf(_currentOpenedPopUp) is false)
                popUpBase.transform.SetAsLastSibling();

            await popUpBase.ShowAsync();

            if (_currentOpenedPopUp)
            {
                _previousOpenedPopUps.Push(_currentOpenedPopUp);

                if (popUpBase.IsSingle)
                    _currentOpenedPopUp.HideImmediately();
            }

            _currentOpenedPopUp = popUpBase;
            CurrentPopUpType = popUpEnum;
        }

        private void HideAllPopUps()
        {
            CurrentPopUpType = default;

            if (_currentOpenedPopUp)
            {
                _currentOpenedPopUp.HideImmediately();
                _currentOpenedPopUp = null;
            }

            _previousOpenedPopUps.Clear();
        }

        private void HandlePopUpHide()
        {
            CurrentPopUpType = default;

            var needShow = _currentOpenedPopUp && _currentOpenedPopUp.IsSingle;
            _currentOpenedPopUp = null;

            if (_previousOpenedPopUps.TryPop(out var previousPopUp) is false)
                return;

            _currentOpenedPopUp = previousPopUp;
            if (needShow)
                previousPopUp.ShowAsync().Forget();
        }
    }
}