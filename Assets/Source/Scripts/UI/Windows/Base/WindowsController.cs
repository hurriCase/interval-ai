using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.Base
{
    internal sealed class WindowsController : SingletonBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _screenPrefabs;
        [SerializeField] private GameObject[] _popUpPrefabs;

        [SerializeField] private Transform _screensContainer;
        [SerializeField] private Transform _popUpsContainer;

        private readonly HashSet<PopUpBase> _createdPopUps = new();
        private readonly HashSet<ScreenBase> _createdScreens = new();
        private readonly Stack<PopUpBase> _previousOpenedPopUps = new();

        private PopUpBase _previousOpenedPopUp;

        public void Init()
        {
            foreach (var screen in _screenPrefabs)
            {
                var createdWindow = Instantiate(screen, _screensContainer);

                if (createdWindow.TryGetComponent<ScreenBase>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);

                screenBase.Init();

                if (screenBase.InitialWindow is false)
                    screenBase.HideImmediately();
            }

            foreach (var popUp in _popUpPrefabs)
            {
                var createdWindow = Instantiate(popUp, _popUpsContainer);

                if (createdWindow.TryGetComponent<PopUpBase>(out var popUpBase) is false)
                    continue;

                _createdPopUps.Add(popUpBase);

                popUpBase.Init();
                popUpBase.OnHidePopUp += ClosePopUp;
                popUpBase.HideImmediately();
            }
        }

        private void ClosePopUp()
        {
            if (_previousOpenedPopUps.TryPop(out var previousWindow) is false)
            {
                _previousOpenedPopUp = null;
                return;
            }

            previousWindow.Show();
        }

        public void OpenPopUpByType(PopUpType popUpType)
        {
            var requestedScreen = _createdPopUps.AsValueEnumerable()
                .FirstOrDefault(screen => screen.UIType == popUpType);

            if (!requestedScreen)
                return;

            if (_previousOpenedPopUp)
                _previousOpenedPopUps.Push(_previousOpenedPopUp);

            _previousOpenedPopUp = requestedScreen;
            requestedScreen.Show();
        }

        public void OpenScreenByType(ScreenType screenType)
        {
            var requestedScreen = _createdScreens.AsValueEnumerable()
                .FirstOrDefault(screen => screen.UIType == screenType);

            if (requestedScreen)
                requestedScreen.Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var window in _createdPopUps)
                window.OnHidePopUp -= ClosePopUp;
        }
    }
}