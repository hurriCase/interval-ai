using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

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

    internal readonly struct UIPool<TData, TPrefab> where TPrefab : MonoBehaviour
    {
        internal IReadOnlyList<TPrefab> PooledItems => _pooledItems;

        private readonly TPrefab _prefab;
        private readonly RectTransform _container;

        private readonly List<TPrefab> _pooledItems;

        private readonly IObjectResolver _objectResolver;
        private readonly UIPoolEvents<TData, TPrefab> _uiPoolEvents;

        internal UIPool(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            IObjectResolver objectResolver = null,
            UIPoolEvents<TData, TPrefab> uiPoolEvents = default)
        {
            _prefab = prefab;
            _container = container;
            _objectResolver = objectResolver;
            _uiPoolEvents = uiPoolEvents;
            _pooledItems = new List<TPrefab>();
        }

        internal IReadOnlyList<TPrefab> EnsureCount(IReadOnlyList<TData> data)
        {
            for (var i = data.Count; i < _pooledItems.Count; i++)
            {
                _pooledItems[i].gameObject.SetActive(false);
                _uiPoolEvents.OnDeactivated?.Invoke(_pooledItems[i]);
            }

            for (var i = _pooledItems.Count; i < data.Count; i++)
            {
                var createdItem = _objectResolver == null
                    ? Object.Instantiate(_prefab, _container)
                    : _objectResolver.Instantiate(_prefab, _container);

                _uiPoolEvents.OnCreated?.Invoke(data[i], createdItem);
                _pooledItems.Add(createdItem);
            }

            for (var i = 0; i < data.Count && i < _pooledItems.Count; i++)
            {
                _pooledItems[i].gameObject.SetActive(true);
                _uiPoolEvents.OnActivated?.Invoke(data[i], _pooledItems[i]);
            }

            return _pooledItems;
        }
    }

    internal readonly struct UIPool<TPrefab> where TPrefab : MonoBehaviour
    {
        internal IReadOnlyList<TPrefab> PooledItems => _pooledItems;

        private readonly TPrefab _prefab;
        private readonly RectTransform _container;

        private readonly List<TPrefab> _pooledItems;

        private readonly IObjectResolver _objectResolver;

        internal UIPool(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            IObjectResolver objectResolver = null)
        {
            _prefab = prefab;
            _container = container;
            _objectResolver = objectResolver;
            _pooledItems = new List<TPrefab>();
        }

        internal IReadOnlyList<TPrefab> EnsureCount(int desiredCount)
        {
            for (var i = desiredCount; i < _pooledItems.Count; i++)
                _pooledItems[i].gameObject.SetActive(false);

            for (var i = _pooledItems.Count; i < desiredCount; i++)
            {
                var createdItem = _objectResolver == null
                    ? Object.Instantiate(_prefab, _container)
                    : _objectResolver.Instantiate(_prefab, _container);

                _pooledItems.Add(createdItem);
            }

            for (var i = 0; i < desiredCount && i < _pooledItems.Count; i++)
                _pooledItems[i].gameObject.SetActive(true);

            return _pooledItems;
        }
    }
}