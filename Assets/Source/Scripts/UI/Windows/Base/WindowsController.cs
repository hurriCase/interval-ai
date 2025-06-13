using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.Base
{
    internal sealed class WindowsController : SingletonBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _windowPrefabs;
        [SerializeField] private GameObject[] _popUpPrefabs;

        private readonly HashSet<PopUpBase> _createdPopUps = new();
        private readonly HashSet<ScreenBase> _createdScreens = new();
        private readonly Stack<PopUpBase> _previousOpenedPopUps = new();

        private PopUpBase _previousOpenedPopUp;

        public void Init()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                if (createdWindow.TryGetComponent<ScreenBase>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);

                screenBase.Init();
                screenBase.HideImmediately();
            }

            foreach (var popUp in _popUpPrefabs)
            {
                var createdWindow = Instantiate(popUp, transform);

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