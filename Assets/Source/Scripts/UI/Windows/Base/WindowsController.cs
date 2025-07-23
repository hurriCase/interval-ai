using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using ZLinq;
using Debug = UnityEngine.Debug;

namespace Source.Scripts.UI.Windows.Base
{
    internal sealed class WindowsController : MonoBehaviour, IWindowsController
    {
        [SerializeField] private AssetReferenceT<GameObject>[] _screenReferences;
        [SerializeField] private AssetReferenceT<GameObject>[] _popUpReferences;

        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popUpsContainer;

        [Inject] private IObjectResolver _objectResolver;

        private readonly HashSet<PopUpBase> _createdPopUps = new();
        private readonly HashSet<ScreenBase> _createdScreens = new();
        private readonly Stack<PopUpBase> _previousOpenedPopUps = new();

        private PopUpBase _currentOpenedPopUp;
        private ScreenBase _currentScreen;

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var sourceWithDestroy = cancellationToken.CreateLinkedTokenSourceWithDestroy(this);

            foreach (var screenReference in _screenReferences)
            {
                var loadedScreen = await PrefabLoader.LoadAsync<GameObject>(screenReference, sourceWithDestroy.Token);
                var createdWindow = _objectResolver.Instantiate(loadedScreen, _screensContainer);

                if (createdWindow.TryGetComponent<ScreenBase>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);

                screenBase.BaseInit();
                screenBase.Init();

                if (screenBase.InitialWindow is false)
                    screenBase.HideImmediately();
            }

            foreach (var popUpReference in _popUpReferences)
            {
                var loadedPopUp = await PrefabLoader.LoadAsync<GameObject>(popUpReference, sourceWithDestroy.Token);
                var createdWindow = _objectResolver.Instantiate(loadedPopUp, _popUpsContainer);

                if (createdWindow.TryGetComponent<PopUpBase>(out var popUpBase) is false)
                    continue;

                _createdPopUps.Add(popUpBase);

                popUpBase.BaseInit();
                popUpBase.Init();
                popUpBase.HideImmediately();
                popUpBase.OnHidePopUp
                    .Subscribe(this, static (_, controller) => controller.HandlePopUpHide())
                    .RegisterTo(sourceWithDestroy.Token);
            }
        }

        public void OpenPopUpByType(PopUpType popUpType)
        {
            foreach (var popUpBase in _createdPopUps)
            {
                if (popUpBase.WindowType != popUpType)
                    continue;

                if (_currentOpenedPopUp)
                {
                    _previousOpenedPopUps.Push(_currentOpenedPopUp);
                    _currentOpenedPopUp.HideImmediately();
                }

                _currentOpenedPopUp = popUpBase;
                popUpBase.Show();
                return;
            }

            Debug.LogError($"[WindowsController::OpenPopUpByType] There is no pop up with type '{popUpType}'");
        }

        public void OpenScreenByType(ScreenType screenType)
        {
            foreach (var screenBase in _createdScreens)
            {
                if (screenBase.WindowType != screenType)
                    continue;

                if (_currentScreen)
                    _currentScreen.Hide();

                _currentScreen = screenBase;
                screenBase.Show();
                return;
            }

            Debug.LogError($"[WindowsController::OpenScreenByType] There is no screen with type '{screenType}'");
        }

        public ScreenType GetInitialScreenType()
        {
            foreach (var screenBase in _createdScreens.AsValueEnumerable())
            {
                if (screenBase.InitialWindow)
                    return screenBase.WindowType;
            }

            return ScreenType.None;
        }

        private void HandlePopUpHide()
        {
            _currentOpenedPopUp = null;

            if (_previousOpenedPopUps.TryPop(out var previousPopUp) is false)
                return;

            _currentOpenedPopUp = previousPopUp;
            previousPopUp.Show();
        }
    }
}