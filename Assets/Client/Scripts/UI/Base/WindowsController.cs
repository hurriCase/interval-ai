using System.Collections.Generic;
using System.Linq;
using Client.Scripts.UI.Base.UITypes;
using CustomClasses.Runtime.Singletons;
using UnityEngine;

namespace Client.Scripts.UI.Base
{
    internal sealed class WindowsController : SingletonBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _windowPrefabs;

        private readonly HashSet<UIBase<WindowType>> _createdWindows = new();
        private readonly HashSet<UIBase<ScreenType>> _createdScreens = new();
        private readonly Stack<UIBase<WindowType>> _previousOpenedWindows = new();

        private UIBase<WindowType> _previousOpenedWindow;

        internal void Init()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                if (createdWindow.TryGetComponent<UIBase<WindowType>>(out var windowBase))
                {
                    _createdWindows.Add(windowBase);
                    windowBase.OnHideWindow += CloseWindow;
                    windowBase.Hide();
                }

                if (createdWindow.TryGetComponent<UIBase<ScreenType>>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);
                screenBase.Hide();
            }
        }

        private void CloseWindow()
        {
            if (_previousOpenedWindows.TryPop(out var previousWindow) is false)
                return;

            previousWindow.Show();
        }

        internal void OpenWindowByType(WindowType windowType)
        {
            var requestedScreen
                = _createdWindows.FirstOrDefault(screen => screen.UIType == windowType);

            if (!requestedScreen)
                return;

            requestedScreen.Show();

            if (!_previousOpenedWindow)
                return;

            _previousOpenedWindows.Push(_previousOpenedWindow);
            _previousOpenedWindow = requestedScreen;
        }

        internal void OpenScreenByType(ScreenType screenType)
        {
            var requestedScreen
                = _createdScreens.FirstOrDefault(screen => screen.UIType == screenType);

            if (requestedScreen)
                requestedScreen.Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var window in _createdWindows)
                window.OnHideWindow -= CloseWindow;
        }
    }
}