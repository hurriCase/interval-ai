using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.Screen;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

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

        [Inject] private IWindowsController _windowsController;
        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private ICategoryStateMutator _categoryStateMutator;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IObjectResolver _objectResolver;

        private EnumArray<CategoryType, CategoryContainerItem> _createdCategoriesByType = new(EnumMode.SkipFirst);
        private readonly Dictionary<CategoryEntry, CategoryEntryItem> _createdCategoryItems = new();

        private readonly Queue<CategoryEntryItem> _cachedCategoryItems = new();

        internal override void Init()
        {
            CreateCategories();

            _spacing.CreateSpacing(_menuSpacingRatio, _categoryItemContainer,
                AspectRatioFitter.AspectMode.WidthControlsHeight);

            _addCategoryButton.OnClickAsObservable()
                .Subscribe(this, (_, self)
                    => self._windowsController.OpenPopUpByType(PopUpType.CategoryCreation))
                .RegisterTo(destroyCancellationToken);

            _categoriesRepository.CategoryAdded
                .Subscribe(this, (entry, self) => self.CreateCategory(entry))
                .RegisterTo(destroyCancellationToken);

            _categoriesRepository.CategoryRemoved
                .Subscribe(this, (entry, self) => self.RemoveCategory(entry))
                .RegisterTo(destroyCancellationToken);

            _categoryStateMutator.CategoryNameChanged
                .Subscribe(this, (entry, self) => self._createdCategoryItems[entry].UpdateName())
                .RegisterTo(destroyCancellationToken);
        }

        private void CreateCategories()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
                CreateCategory(categoryEntry);
        }

        private void CreateCategory(CategoryEntry categoryEntry)
        {
            var categoryType = categoryEntry.CategoryType;
            var categoryContainer = _createdCategoriesByType[categoryType];
            if (!categoryContainer)
                categoryContainer = CreateCategoriesContainer(categoryType);

            if (_cachedCategoryItems.TryDequeue(out var createdCategory))
            {
                createdCategory.SetActive(true);
                createdCategory.transform.SetParent(categoryContainer.CategoryContainer);
            }
            else
                createdCategory = _objectResolver
                    .Instantiate(_categoryEntryItem, categoryContainer.CategoryContainer);

            createdCategory.Init(categoryEntry);
            _createdCategoryItems[categoryEntry] = createdCategory;
        }

        private void RemoveCategory(CategoryEntry categoryEntry)
        {
            if (_createdCategoryItems.TryGetValue(categoryEntry, out var createdCategory) is false)
                return;

            createdCategory.SetActive(false);
            _createdCategoryItems.Remove(categoryEntry);
            _cachedCategoryItems.Enqueue(createdCategory);
        }

        private CategoryContainerItem CreateCategoriesContainer(CategoryType categoryType)
        {
            var categoryContainer = _objectResolver.Instantiate(_categoryContainerItemPrefab, _categoryItemContainer);
            categoryContainer.TitleText.text =
                _localizationKeysDatabase.GetLearningStateLocalization(categoryType);

            _spacing.CreateSpacing(_categoryContainerSpacingRatio, _categoryItemContainer,
                AspectRatioFitter.AspectMode.WidthControlsHeight);
            _createdCategoriesByType[categoryType] = categoryContainer;
            return categoryContainer;
        }
    }
}