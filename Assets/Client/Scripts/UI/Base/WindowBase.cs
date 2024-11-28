using System;
using UnityEngine;

namespace Client.Scripts.UI.Base
{
    [Serializable]
    internal abstract class WindowBase : MonoBehaviour
    {
        internal bool CanHide { get; set; } = true;

        internal static void Show<T>() where T : WindowBase => WindowsController.Instance.OpenWindow<T>();

        internal void Hide() => gameObject.SetActive(false);
    }
}