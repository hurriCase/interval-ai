using System.Collections.Generic;
using Client.Scripts.Patterns.Singletons;
using UnityEngine;

namespace Client.Scripts.UI.Base
{
    internal sealed class WindowsController : SingletonMonoBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _windowPrefabs;

        private readonly HashSet<WindowBase> _createdWindows = new();

        internal void Init()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                createdWindow.SetActive(false);

                if (createdWindow.TryGetComponent<WindowBase>(out var windowBase))
                    _createdWindows.Add(windowBase);
            }
        }

        internal void OpenWindow<T>() where T : WindowBase
        {
            foreach (var createdWindow in _createdWindows)
            {
                if (createdWindow.GetType() == typeof(T))
                    createdWindow.gameObject.SetActive(true);
                else if (createdWindow.CanHide)
                    createdWindow.gameObject.SetActive(false);
            }
        }
    }
}