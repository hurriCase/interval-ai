using System.Collections.Generic;
using CustomUtils.Runtime.Extensions.Observables;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Base;
using TMPro;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.Shared
{
    internal abstract class SearchBase<TEntry> : MonoBehaviour where TEntry : IEntry
    {
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private RectTransform _container;
        [SerializeField] private View<TEntry> _displayItem;

        [Inject] private IObjectResolver _objectResolver;

        protected abstract Dictionary<int, TEntry> SearchResults { get; }

        private UIPoolWithData<TEntry, View<TEntry>> _itemsPool;

        internal void Init()
        {
            var poolEvents = new UIPoolEvents<TEntry, View<TEntry>>(
                static (entry, view) => view.Init(entry),
                static (entry, view) => view.UpdateView(entry));

            _itemsPool = new UIPoolWithData<TEntry, View<TEntry>>(_displayItem, _container, poolEvents, _objectResolver);

            _searchInputField.OnValueChangedAsObservable()
                .SubscribeUntilDestroy(this, static (searchText, self) => self.UpdateSearchResults(searchText));

            UpdateSearchResults(string.Empty);
        }

        internal void SelectInput()
        {
            _searchInputField.Select();
            _searchInputField.ActivateInputField();
        }

        private void UpdateSearchResults(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _itemsPool.EnsureCount(SearchResults.Values);
                return;
            }

            var lowerSearchText = searchText.ToLower();

            using var filteredCategories = SearchResults.Values
                .Where(category => category.GetName().ToLower().Contains(lowerSearchText))
                .ToArrayPool();

            var filteredCategoriesSpan = filteredCategories.Span;
            _itemsPool.EnsureCount(filteredCategoriesSpan);
        }
    }
}