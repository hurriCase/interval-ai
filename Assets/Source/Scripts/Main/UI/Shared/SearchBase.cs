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

        private UIPool<View<TEntry>> _itemsPool;

        internal void Init()
        {
            _itemsPool = new UIPool<View<TEntry>>(_displayItem, _container, _objectResolver);

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
                ShowAllCategories();
                return;
            }

            UpdateCategories(searchText);
        }

        private void ShowAllCategories()
        {
            _itemsPool.EnsureCount(SearchResults.Count);
            var index = 0;
            foreach (var categoryEntry in SearchResults.Values)
            {
                _itemsPool.ActiveItems[index].UpdateView(categoryEntry);
                index++;
            }
        }

        private void UpdateCategories(string searchText)
        {
            var lowerSearchText = searchText.ToLower();

            using var filteredCategories = SearchResults.Values
                .Where(category => category.GetName().ToLower().Contains(lowerSearchText))
                .ToArrayPool();

            var filteredCategoriesSpan = filteredCategories.Span;
            _itemsPool.EnsureCount(filteredCategoriesSpan.Length);

            for (var i = 0; i < _itemsPool.ActiveItems.Count; i++)
            {
                var categoriesPoolActiveItem = _itemsPool.ActiveItems[i];
                categoriesPoolActiveItem.UpdateView(filteredCategoriesSpan[i]);
            }
        }
    }
}