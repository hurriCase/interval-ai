using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Base;
using TMPro;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryContainerBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform _categoryContainer;
        [SerializeField] private CategoryEntryItem _categoryEntryItem;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private ThemeButton _addCategoryButton;

        private CategoryType _currentCategoryType;

        private UIPoolWithData<CategoryEntry, CategoryEntryItem> _categoriesPool;

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private ICategoriesRepository _categoriesRepository;
        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(
            ILocalizationKeysDatabase localizationKeysDatabase,
            ICategoriesRepository categoriesRepository,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
            _categoriesRepository = categoriesRepository;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal void Init(CategoryType categoryType)
        {
            _currentCategoryType = categoryType;

            CreatePool();

            if (categoryType == CategoryType.Created)
                _windowsController.BindPopUpOpen(_addCategoryButton, PopUpType.CategoryCreation);
            else
                _addCategoryButton.SetActive(false);

            _categoriesRepository.OnCategoryAdded
                .SubscribeUntilDestroy(this, static (entry, self) => self._categoriesPool.AddElement(entry));

            _categoriesRepository.OnCategoryRemoved
                .SubscribeUntilDestroy(this, static (entry, self) => self._categoriesPool.RemoveElement(entry));

            LocalizationController.Language.SubscribeUntilDestroy(this, static self => self.UpdateTitleText());
        }

        private void CreatePool()
        {
            var poolEvents = new UIPoolEvents<CategoryEntry, CategoryEntryItem>(
                static (entry, categoryItem) => categoryItem.Init(entry),
                onDeactivated: _ => gameObject.SetActive(_categoriesPool.ActiveItems.Count > 0));

            _categoriesPool =
                new UIPoolWithData<CategoryEntry, CategoryEntryItem>(_categoryEntryItem, _categoryContainer, poolEvents,
                    _objectResolver);

            using var categoryEntries = _categoriesRepository.CategoryEntries.CurrentValue.Values
                .Where(entry => entry.CategoryType == _currentCategoryType)
                .ToArrayPool();

            _categoriesPool.EnsureCount(categoryEntries.Span);
            gameObject.SetActive(categoryEntries.Span.Length > 0);
        }

        private void UpdateTitleText()
        {
            _titleText.text = _localizationKeysDatabase.GetLearningStateLocalization(_currentCategoryType);
        }
    }
}