using System.Collections.Generic;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryScreen : ScreenBase
    {
        [SerializeField] private CategoryContainerItem _categoryContainerItemPrefab;
        [SerializeField] private CategoryEntryItem _categoryEntryItem;
        [SerializeField] private RectTransform _categoryItemContainer;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _spacingRatio;

        private readonly Dictionary<CategoryType, CategoryContainerItem> _createdCategoriesByType = new();

        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal override void Init()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.Value)
            {
                var categoryType = categoryEntry.CategoryType;
                if (_createdCategoriesByType
                        .TryGetValue(categoryType, out var createdCategoryContainer) is false)
                {
                    createdCategoryContainer = Instantiate(_categoryContainerItemPrefab, _categoryItemContainer);
                    createdCategoryContainer.TitleText.text =
                        _localizationKeysDatabase.GetLearningStateLocalization(categoryType);

                    var createdSpacing = Instantiate(_spacing, _categoryItemContainer);
                    createdSpacing.aspectRatio = _spacingRatio;
                }

                var createdCategory = Instantiate(_categoryEntryItem, createdCategoryContainer.CategoryContainer);
                createdCategory.Init(categoryEntry);
                _createdCategoriesByType.TryAdd(categoryType, createdCategoryContainer);
            }
        }
    }
}