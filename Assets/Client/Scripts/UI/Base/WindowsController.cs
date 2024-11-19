using System.Collections.Generic;
using Client.Scripts.Patterns;
using UnityEngine;

namespace Client.Scripts.UI
{
    internal class WindowsController : SingletonMonoBehaviour<WindowsController>
    {
        [SerializeField] private GameObject[] _windowPrefabs;

        private readonly List<GameObject> _windows = new List<GameObject>();

        private void Start()
        {
            foreach (var window in _windowPrefabs)
            {
                var createdWindow = Instantiate(window, transform);

                createdWindow.SetActive(false);

                _windows.Add(createdWindow);
            }
        }

        internal void OpenWindow(WindowBase window)
        {
            foreach (var createdWindow in _windows)
            {
                if (createdWindow.TryGetComponent<WindowBase>(out var windowBase))
                    window.gameObject.SetActive(windowBase == window);
                else if (window.CanHide is false)
                    window.gameObject.SetActive(false);
            }
        }
    }
}