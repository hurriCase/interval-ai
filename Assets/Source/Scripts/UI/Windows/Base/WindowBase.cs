using System;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    [Serializable]
    [RequireComponent(typeof(CanvasGroup))]
    internal abstract class WindowBase<T> : CanvasGroupBehaviour where T : Enum
    {
        //TODO:<Dmitriy.Sukharev> make setter private in some way
        [field: SerializeField] internal bool InitialWindow { get; private set; }
        internal T WindowType { get; set; }

        internal virtual void BaseInit() { }

        internal virtual void Init() { }

        internal abstract void Show();
        internal abstract void Hide();

        internal virtual void HideImmediately() => CanvasGroup.Hide();
    }
}