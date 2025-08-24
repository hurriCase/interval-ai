using System.Collections.Generic;
using JetBrains.Annotations;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.Others
{
    internal readonly struct UIPool<TPrefab> where TPrefab : MonoBehaviour
    {
        internal IReadOnlyList<TPrefab> PooledItems => _pooledItems;
        internal Observable<TPrefab> CreateObservable => _createSubject;
        private readonly Subject<TPrefab> _createSubject;

        private readonly TPrefab _prefab;
        private readonly RectTransform _container;

        private readonly List<TPrefab> _pooledItems;

        private readonly AspectRatioFitter _spacing;
        private readonly float _spacingRatio;
        private readonly AspectRatioFitter.AspectMode _aspectMode;

        private readonly IObjectResolver _objectResolver;

        internal UIPool(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            AspectRatioFitter spacing = null,
            float spacingRatio = 0,
            AspectRatioFitter.AspectMode aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth,
            IObjectResolver objectResolver = null)
        {
            _prefab = prefab;
            _container = container;
            _objectResolver = objectResolver;
            _pooledItems = new List<TPrefab>();
            _createSubject = new Subject<TPrefab>();
            _spacing = spacing;
            _spacingRatio = spacingRatio;
            _aspectMode = aspectMode;
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
                _createSubject.OnNext(createdItem);

                if (_spacing && _spacingRatio > 0)
                    _spacing.CreateSpacing(_spacingRatio, _container, _aspectMode);
            }

            for (var i = 0; i < desiredCount && i < _pooledItems.Count; i++)
                _pooledItems[i].gameObject.SetActive(true);

            return _pooledItems;
        }
    }
}