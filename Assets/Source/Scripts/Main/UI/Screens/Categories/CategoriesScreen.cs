using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Base.PopUp;
using Source.Scripts.UI.Windows.Base.Screen;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoriesScreen : ScreenBase
    {
        [SerializeField] private ButtonComponent _addCategoryButton;

        [SerializeField] private CategoryContainerItem _categoryContainerItemPrefab;
        [SerializeField] private CategoryEntryItem _categoryEntryItem;
        [SerializeField] private RectTransform _categoryItemContainer;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _categoryContainerSpacingRatio;
        [SerializeField] private float _menuSpacingRatio;

        private EnumArray<CategoryType, CategoryContainerItem> _createdCategoriesByType = new(EnumMode.SkipFirst);

        [Inject] private IWindowsController _windowsController;
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal override void Init()
        {
            CreateCategories();

            _spacing.CreateSpacing(_menuSpacingRatio, _categoryItemContainer,
                AspectRatioFitter.AspectMode.WidthControlsHeight);

            _addCategoryButton.OnClickAsObservable()
                .Subscribe(this, (_, self)
                    => self._windowsController.OpenPopUpByType(PopUpType.CategoryCreation))
                .RegisterTo(destroyCancellationToken);
        }

        private void CreateCategories()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
            {
                var categoryType = categoryEntry.CategoryType;
                var categoryContainer = _createdCategoriesByType[categoryType];
                if (!categoryContainer)
                    categoryContainer = CreateCategoriesContainer(categoryType);

                var createdCategory = Instantiate(_categoryEntryItem, categoryContainer.CategoryContainer);
                createdCategory.Init(categoryEntry);
            }
        }

        private CategoryContainerItem CreateCategoriesContainer(CategoryType categoryType)
        {
            var categoryContainer = Instantiate(_categoryContainerItemPrefab, _categoryItemContainer);
            categoryContainer.TitleText.text =
                _localizationKeysDatabase.GetLearningStateLocalization(categoryType);

            _spacing.CreateSpacing(_categoryContainerSpacingRatio, _categoryItemContainer,
                AspectRatioFitter.AspectMode.WidthControlsHeight);
            _createdCategoriesByType[categoryType] = categoryContainer;
            return categoryContainer;
        }
    }
}