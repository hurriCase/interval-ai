using System;
using UnityEngine;

namespace Source.Scripts.Core.Others
{
    internal readonly struct UIPoolEvents<TData, TPrefab> where TPrefab : MonoBehaviour
    {
        internal Action<TData, TPrefab> OnCreated { get; }
        internal Action<TData, TPrefab> OnActivated { get; }
        internal Action<TPrefab> OnDeactivated { get; }

        internal UIPoolEvents(
            Action<TData, TPrefab> onCreated = null,
            Action<TData, TPrefab> onActivated = null,
            Action<TPrefab> onDeactivated = null)
        {
            OnCreated = onCreated;
            OnActivated = onActivated;
            OnDeactivated = onDeactivated;
        }
    }
}