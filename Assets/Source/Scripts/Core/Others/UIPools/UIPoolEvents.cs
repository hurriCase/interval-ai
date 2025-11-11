using System;
using UnityEngine;

namespace Source.Scripts.Core.Others.UIPools
{
    internal readonly struct UIPoolEvents<TData, TPrefab> where TPrefab : MonoBehaviour
    {
        internal Action<TData, TPrefab> OnCreated { get; }
        internal Action<TData, TPrefab> OnUpdate { get; }
        internal Action<TPrefab> OnDeactivated { get; }

        internal UIPoolEvents(
            Action<TData, TPrefab> onCreated = null,
            Action<TData, TPrefab> onUpdate = null,
            Action<TPrefab> onDeactivated = null)
        {
            OnCreated = onCreated;
            OnUpdate = onUpdate;
            OnDeactivated = onDeactivated;
        }
    }
}