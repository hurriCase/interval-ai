using System;
using Client.Scripts.Patterns.DI.Base;

namespace Client.Scripts.UI.Base
{
    [Serializable]
    internal abstract class WindowBase : InjectableBehaviour
    {
        internal bool CanHide { get; set; } = true;

        internal static void Show<T>() where T : WindowBase => WindowsController.Instance.OpenWindow<T>();

        internal void Hide() => gameObject.SetActive(false);
    }
}