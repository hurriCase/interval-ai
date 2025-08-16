using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Loader;
using Source.Scripts.UI.Windows.Base.PopUp;
using Source.Scripts.UI.Windows.Base.Screen;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.UI.Windows.Base
{
    internal abstract class WindowsController<TScreenType, TPopUpType> : MonoBehaviour
        where TScreenType : unmanaged, Enum
        where TPopUpType : unmanaged, Enum
    {
        [SerializeField]
        private EnumArray<TScreenType, AssetReferenceT<GameObject>> _screenReferences = new(EnumMode.SkipFirst);

        [SerializeField]
        private EnumArray<TPopUpType, AssetReferenceT<GameObject>> _popUpReferences = new(EnumMode.SkipFirst);

        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popUpsContainer;

        public TScreenType InitialScreenType { get; private set; }

        private EnumArray<TScreenType, ScreenBase> _createdScreens = new(EnumMode.SkipFirst);
        private EnumArray<TPopUpType, PopUpBase> _createdPopUps = new(EnumMode.SkipFirst);
        private readonly Stack<PopUpBase> _previousOpenedPopUps = new();

        private PopUpBase _currentOpenedPopUp;
        private ScreenBase _currentScreen;

        private TScreenType _initialScreenType;

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
                popUpBase.OnHidePopUp
                    .Subscribe(this, static (_, controller) => controller.HandlePopUpHide())
                    .RegisterTo(cancellationToken);
            }
        }

        public void OpenScreenByType(TScreenType screenType)
        {
            HideAllPopUps();

            var screenBase = _createdScreens[screenType];

            if (!screenBase)
                Debug.LogError($"[WindowsController::OpenScreenByType] There is no screen with type '{screenType}'");

            if (_currentScreen)
                _currentScreen.Hide();

            _currentScreen = screenBase;
            screenBase.Show();
        }

        public void OpenPopUpByType(TPopUpType popUpType)
        {
            if (TryGetPopUp(popUpType, out var popUpBase) is false)
                return;

            popUpBase.Show();
        }

        //TODO:<Dmitriy.Sukharev> refactor
        public void OpenPopUpByType<TParameters>(TPopUpType popUpType, TParameters parameters)
        {
            if (TryGetPopUp(popUpType, out var popUpBase) is false)
                return;

            if (popUpBase is not IParameterizedPopUpBase parameterizedPopUp)
            {
                Debug.LogError("[WindowsController::OpenPopUpByType] " +
                               $"There is no parameterized pop up with type '{popUpType}'");
                return;
            }

            parameterizedPopUp.SetParameters(parameters);
            popUpBase.Show();
        }

        private bool TryGetPopUp(TPopUpType popUpType, out PopUpBase popUpBase)
        {
            popUpBase = _createdPopUps[popUpType];
            if (!popUpBase)
            {
                Debug.LogError($"[WindowsController::OpenPopUpByType] There is no pop up with type '{popUpType}'");
                return false;
            }

            if (_currentOpenedPopUp)
            {
                _previousOpenedPopUps.Push(_currentOpenedPopUp);

                if (popUpBase.IsSingle)
                    _currentOpenedPopUp.HideImmediately();
            }

            _currentOpenedPopUp = popUpBase;
            return true;
        }

        private void HideAllPopUps()
        {
            if (_currentOpenedPopUp)
            {
                _currentOpenedPopUp.HideImmediately();
                _currentOpenedPopUp = null;
            }

            _previousOpenedPopUps.Clear();
        }

        private void HandlePopUpHide()
        {
            var needShow = _currentOpenedPopUp.IsSingle;

            _currentOpenedPopUp = null;

            if (_previousOpenedPopUps.TryPop(out var previousPopUp) is false)
                return;

            _currentOpenedPopUp = previousPopUp;
            if (needShow)
                previousPopUp.Show();
        }
    }
}