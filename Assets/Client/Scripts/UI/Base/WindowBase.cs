using UnityEngine;

namespace Client.Scripts.UI
{
    internal abstract class WindowBase : MonoBehaviour
    {
        internal bool CanHide { get; set; }

        internal void Show(WindowBase type) => WindowsController.Instance.OpenWindow(type);

        internal void Hide() => gameObject.SetActive(false);
    }
}