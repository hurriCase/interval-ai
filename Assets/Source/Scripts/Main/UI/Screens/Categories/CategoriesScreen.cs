using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
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

        private EnumArray<CategoryType, CategoryContainerItem> _createdCategoriesByType = new(EnumMode.SkipFirst);
        private readonly Dictionary<CategoryEntry, CategoryEntryItem> _createdCategoryItems = new();

        private readonly Queue<CategoryEntryItem> _cachedCategoryItems = new();

        private ICategoriesRepository _categoriesRepository;
        private ICategoryStateMutator _categoryStateMutator;
        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(
            ICategoriesRepository categoriesRepository,
            ICategoryStateMutator categoryStateMutator,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _categoriesRepository = categoriesRepository;
            _categoryStateMutator = categoryStateMutator;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            foreach (var categoryEntry in _categoriesRepository.CategoryEntries.CurrentValue.Values)
                CreateCategory(categoryEntry);

            _spacing.CreateHeightSpacing(_menuSpacingRatio, _categoryItemContainer);

            _addCategoryButton.OnClickAsObservable().SubscribeAndRegister(this,
                static self => self._windowsController.OpenPopUpByType(PopUpType.CategoryCreation));

            _categoriesRepository.CategoryAdded.SubscribeAndRegister(this,
                static (entry, self) => self.CreateCategory(entry));

            _categoriesRepository.CategoryRemoved.SubscribeAndRegister(this,
                static (entry, self) => self.RemoveCategory(entry));

            _categoryStateMutator.CategoryNameChanged.SubscribeAndRegister(this,
                static (entry, self) => self._createdCategoryItems[entry].UpdateName());
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
            categoryContainer.Init(categoryType);

            _spacing.CreateHeightSpacing(_categoryContainerSpacingRatio, _categoryItemContainer);
            _createdCategoriesByType[categoryType] = categoryContainer;
            return categoryContainer;
        }
    }
}