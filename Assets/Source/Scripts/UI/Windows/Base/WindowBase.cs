using System;
using CustomUtils.Runtime.Attributes;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Base
{
    [Serializable]
    [RequireComponent(typeof(CanvasGroup))]
    internal abstract class WindowBase : MonoBehaviour
    {
        [field: SerializeField] internal bool InitialWindow { get; private set; }

        [SerializeField, Self] protected CanvasGroup canvasGroup;

        internal virtual void BaseInit() { }

        internal virtual void Init() { }

        internal abstract UniTask ShowAsync();
        internal abstract UniTask HideAsync();

        internal virtual void HideImmediately() => canvasGroup.Hide();
    }
}