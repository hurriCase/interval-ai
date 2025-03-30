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
        private readonly Stack<UIBase<WindowType>> _openedWindows = new();

        internal void Init()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                if (createdWindow.TryGetComponent<UIBase<WindowType>>(out var windowBase))
                {
                    _createdWindows.Add(windowBase);
                    windowBase.Hide();
                }

                if (createdWindow.TryGetComponent<UIBase<ScreenType>>(out var screenBase) is false)
                    continue;

                _createdScreens.Add(screenBase);
                screenBase.Hide();
            }
        }

        internal void OpenWindowByType(WindowType windowType)
        {
            var requestedScreen
                = _createdWindows.FirstOrDefault(screen => screen.UIType == windowType);

            if (!requestedScreen)
                return;

            requestedScreen.Show();
            _openedWindows.Push(requestedScreen);
        }

        internal void CloseTopWindow()
        {
            var topWindow = _openedWindows.Pop();
            topWindow.Hide();
        }

        internal void OpenScreenByType(ScreenType screenType)
        {
            var requestedScreen
                = _createdScreens.FirstOrDefault(screen => screen.UIType == screenType);

            if (requestedScreen)
                requestedScreen.Show();
        }
    }
}