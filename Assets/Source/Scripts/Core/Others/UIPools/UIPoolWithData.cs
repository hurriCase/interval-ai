using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.Others.UIPools
{
    internal sealed class UIPoolWithData<TData, TPrefab> : UIPool<TPrefab> where TPrefab : MonoBehaviour
    {
        private readonly UIPoolEvents<TData, TPrefab> _uiPoolEvents;

        private IReadOnlyList<TData> _currentData;

        internal UIPoolWithData(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            UIPoolEvents<TData, TPrefab> uiPoolEvents,
            IObjectResolver objectResolver = null) : base(prefab, container, objectResolver)
        {
            _uiPoolEvents = uiPoolEvents;
        }

        internal void EnsureCount(List<TData> data)
        {
            _currentData = data;

            EnsureCount(data.Count);
        }

        protected override void OnDeactivatePrefab(TPrefab prefab)
        {
            _uiPoolEvents.OnDeactivated?.Invoke(prefab);
        }

        protected override void OnCreatePrefab(TPrefab prefab, int index)
        {
            _uiPoolEvents.OnCreated?.Invoke(_currentData[index], prefab);
        }

        protected override void OnActivatePrefab(TPrefab prefab, int index)
        {
            _uiPoolEvents.OnActivated?.Invoke(_currentData[index], prefab);
        }
    }
}