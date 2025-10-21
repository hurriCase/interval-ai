using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Main.UI.Screens.Categories;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.PopUps.Search
{
    internal sealed class SearchPopUp : PopUpBase
    {
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private RectTransform _categoriesContainer;
        [SerializeField] private CategoryEntryItem _categoryItem;

        private UIPool<CategoryEntryItem> _categoriesPool;
        private ICategoriesRepository _categoriesRepository;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(ICategoriesRepository categoriesRepository, IObjectResolver objectResolver)
        {
            _categoriesRepository = categoriesRepository;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            _categoriesPool = new UIPool<CategoryEntryItem>(_categoryItem, _categoriesContainer, _objectResolver);

            _searchInputField.OnValueChangedAsObservable()
                .SubscribeUntilDestroy(this, static (searchText, self) => self.UpdateSearchResults(searchText));

            UpdateSearchResults(string.Empty);
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            _searchInputField.Select();
            _searchInputField.ActivateInputField();
        }

        private void UpdateSearchResults(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _categoriesPool.EnsureCount(_categoriesRepository.CategoryEntries.CurrentValue.Values.Count);
                var index = 0;
                foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
                {
                    _categoriesPool.ActiveItems[index].UpdateView(categoryEntry);
                    index++;
                }

                return;
            }

            var lowerSearchText = searchText.ToLower();

            using var filteredCategories = _categoriesRepository.CategoryEntries.CurrentValue.Values
                .Where(category => category.Name.ToLower().Contains(lowerSearchText))
                .ToArrayPool();

            var filteredCategoriesSpan = filteredCategories.Span;
            _categoriesPool.EnsureCount(filteredCategoriesSpan.Length);

            for (var i = 0; i < _categoriesPool.ActiveItems.Count; i++)
            {
                var categoriesPoolActiveItem = _categoriesPool.ActiveItems[i];
                categoriesPoolActiveItem.UpdateView(filteredCategoriesSpan[i]);
            }
        }
    }
}