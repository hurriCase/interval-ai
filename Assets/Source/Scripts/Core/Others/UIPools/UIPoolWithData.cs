using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZLinq;
using Object = UnityEngine.Object;

namespace Source.Scripts.Core.Others.UIPools
{
    internal sealed class UIPoolWithData<TData, TPrefab> where TPrefab : MonoBehaviour
    {
        internal IReadOnlyList<TPrefab> ActiveItems => _activeItems;

        private readonly List<TPrefab> _activeItems = new();
        private readonly List<TData> _activeData = new();
        private readonly Queue<TPrefab> _inactiveItems = new();

        private readonly TPrefab _prefab;
        private readonly RectTransform _container;
        private readonly IObjectResolver _objectResolver;
        private readonly UIPoolEvents<TData, TPrefab> _events;

        internal UIPoolWithData(
            [NotNull] TPrefab prefab,
            [NotNull] RectTransform container,
            UIPoolEvents<TData, TPrefab> events,
            IObjectResolver objectResolver = null)
        {
            _prefab = prefab;
            _container = container;
            _events = events;
            _objectResolver = objectResolver;
        }

        internal void AddElement(TData data)
        {
            var item = GetOrCreateItem(data);

            _activeItems.Add(item);
            _activeData.Add(data);
        }

        internal void RemoveElement(TData data)
        {
            var index = _activeData.IndexOf(data);
            var item = _activeItems[index];

            _activeItems.RemoveAt(index);
            _activeData.RemoveAt(index);

            _events.OnDeactivated?.Invoke(item);

            item.gameObject.SetActive(false);
            _inactiveItems.Enqueue(item);
        }

        internal void EnsureCount(Span<TData> data)
        {
            while (_activeItems.Count > data.Length)
            {
                RemoveElement(_activeData[^1]);
            }

            for (var i = _activeItems.Count; i < data.Length; i++)
                AddElement(data[i]);

            for (var i = 0; i < data.Length; i++)
                _events.OnActivated?.Invoke(data[i], _activeItems[i]);
        }

        internal void EnsureCount(IReadOnlyCollection<TData> dataList)
        {
            while (_activeItems.Count > dataList.Count)
            {
                RemoveElement(_activeData[^1]);
            }

            foreach (var item in dataList.Skip(_activeItems.Count))
                AddElement(item);

            var index = 0;
            foreach (var data in dataList)
            {
                _events.OnActivated?.Invoke(data, _activeItems[index]);
                index++;
            }
        }

        private TPrefab GetOrCreateItem(TData data)
        {
            if (_inactiveItems.TryDequeue(out var item))
            {
                item.gameObject.SetActive(true);
                return item;
            }

            var newItem = _objectResolver == null
                ? Object.Instantiate(_prefab, _container)
                : _objectResolver.Instantiate(_prefab, _container);

            _events.OnCreated?.Invoke(data, newItem);

            return newItem;
        }
    }
}