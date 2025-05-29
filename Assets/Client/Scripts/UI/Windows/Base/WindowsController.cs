using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using UnityEngine;
using ZLinq;

namespace Client.Scripts.UI.Windows.Base
{
    internal sealed class WindowsController : SingletonBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _windowPrefabs;

        private readonly HashSet<WindowBase<PopUpType>> _createdPopUps = new();
        private readonly HashSet<WindowBase<ScreenType>> _createdScreens = new();
        private readonly Stack<WindowBase<PopUpType>> _previousOpenedPopUps = new();

        private WindowBase<PopUpType> _previousOpenedPopUp;

        internal void Init()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                if (createdWindow.TryGetComponent<WindowBase<PopUpType>>(out var windowBase))
                {
                    _createdPopUps.Add(windowBase);
                    windowBase.OnHideWindow += CloseWindow;
                    windowBase.Hide();
                }

                if (createdWindow.TryGetComponent<WindowBase<ScreenType>>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);
                screenBase.Hide();
            }
        }

        private void CloseWindow()
        {
            if (_previousOpenedPopUps.TryPop(out var previousWindow) is false)
                return;

            previousWindow.Show();
        }

        internal void OpenWindowByType(PopUpType popUpType)
        {
            var requestedScreen = _createdPopUps
                .AsValueEnumerable().FirstOrDefault(screen => screen.UIType == popUpType);

            if (!requestedScreen)
                return;

            requestedScreen.Show();

            if (!_previousOpenedPopUp)
                return;

            _previousOpenedPopUps.Push(_previousOpenedPopUp);
            _previousOpenedPopUp = requestedScreen;
        }

        internal void OpenScreenByType(ScreenType screenType)
        {
            var requestedScreen = _createdScreens
                .AsValueEnumerable().FirstOrDefault(screen => screen.UIType == screenType);

            if (requestedScreen)
                requestedScreen.Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var window in _createdPopUps)
                window.OnHideWindow -= CloseWindow;
        }
    }
}