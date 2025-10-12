using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Source.Scripts.Core.Others.UIPools
{
    internal class UIPool<TPrefab> where TPrefab : MonoBehaviour
    {
        internal IReadOnlyList<TPrefab> PooledItems => _pooledItems;
        private readonly List<TPrefab> _pooledItems;

        private readonly TPrefab _prefab;
        private readonly RectTransform _container;

        private readonly IObjectResolver _objectResolver;

        internal UIPool(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            IObjectResolver objectResolver = null)
        {
            _pooledItems = new List<TPrefab>();
            _prefab = prefab;
            _container = container;
            _objectResolver = objectResolver;
        }

        internal void EnsureCount(int desiredCount)
        {
            for (var i = desiredCount; i < _pooledItems.Count; i++)
            {
                _pooledItems[i].gameObject.SetActive(false);
                OnDeactivatePrefab(_pooledItems[i]);
            }

            for (var i = _pooledItems.Count; i < desiredCount; i++)
            {
                var createdItem = _objectResolver == null
                    ? Object.Instantiate(_prefab, _container)
                    : _objectResolver.Instantiate(_prefab, _container);

                _pooledItems.Add(createdItem);
                OnCreatePrefab(_pooledItems[i], i);
            }

            for (var i = 0; i < desiredCount && i < _pooledItems.Count; i++)
            {
                _pooledItems[i].gameObject.SetActive(true);
                OnActivatePrefab(_pooledItems[i], i);
            }
        }

        protected virtual void OnDeactivatePrefab(TPrefab prefab) { }
        protected virtual void OnCreatePrefab(TPrefab prefab, int index) { }
        protected virtual void OnActivatePrefab(TPrefab prefab, int index) { }
    }
}