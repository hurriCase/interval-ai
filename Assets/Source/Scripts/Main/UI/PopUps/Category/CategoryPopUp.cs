using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Modal;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class CategoryPopUp : PopUpBase
    {
        [SerializeField] private InputFieldComponent _categoryNameText;

        [SerializeField] private ButtonComponent _deleteButton;

        [SerializeField] private ButtonComponent _resetProgressButton;

        [SerializeField] private RectTransform _wordsContainer;
        [SerializeField] private WordItem _wordItem;

        [SerializeField] private SelectionItem _wordOrderSelectionItem;

        private readonly ReactiveProperty<WordOrderType> _wordReviewSourceType = new(WordOrderType.Default);
        private CategoryEntry _currentCategoryEntry;

        private UIPool<WordEntry, WordItem> _wordsPool;

        private ILocalizationDatabase _localizationDatabase;
        private ICategoriesRepository _categoriesRepository;
        private ICategoryStateMutator _categoryStateMutator;
        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        public void Inject(
            ILocalizationDatabase localizationDatabase,
            ICategoriesRepository categoriesRepository,
            ICategoryStateMutator categoryStateMutator,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _localizationDatabase = localizationDatabase;
            _categoriesRepository = categoriesRepository;
            _categoryStateMutator = categoryStateMutator;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal void SetParameters(CategoryEntry categoryEntry)
        {
            _currentCategoryEntry = categoryEntry;

            _wordReviewSourceType.Value = _currentCategoryEntry.WordOrderType;

            UpdateView();
        }

        internal override void Init()
        {
            var poolConfig = new UIPoolEvents<WordEntry, WordItem>(
                (_, wordItem) => wordItem.Init(),
                (wordEntry, wordItem) => wordItem.UpdateView(wordEntry));

            _wordsPool = new UIPool<WordEntry, WordItem>(_wordItem, _wordsContainer, _objectResolver, poolConfig);

            _deleteButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self =>
                self.OpenWarning(ModalLocalizationType.DeleteCategory, static self => self.RemoveCategory()));

            _resetProgressButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self =>
                self.OpenWarning(ModalLocalizationType.ResetProgress,
                    static self => self._categoryStateMutator.ResetWordsProgress(self._currentCategoryEntry)));

            _categoryNameText.OnTextChanged
                .SubscribeUntilDestroy(this, static (newName, self)
                    => self._categoryStateMutator.ChangeCategoryName(self._currentCategoryEntry, newName));

            _wordReviewSourceType
                .Where(this, static (_, self) => self._currentCategoryEntry != null)
                .SubscribeUntilDestroy(this, static (newOrder, self) => self.ReorderWordItems(newOrder));

            _wordOrderSelectionItem.Init(_wordReviewSourceType);
        }

        private void OpenWarning(ModalLocalizationType localizationType, Action<CategoryPopUp> positiveAction)
        {
            var modalPopUp = _windowsController.OpenPopUp<ModalPopUp>();
            var localization = _localizationDatabase.ModalLocalizations[localizationType];
            modalPopUp.SetParameters(localization, this, positiveAction);
        }

        private void RemoveCategory()
        {
            _categoriesRepository.RemoveCategory(_currentCategoryEntry);

            HideAsync().Forget();
        }

        private void UpdateView()
        {
            _categoryNameText.text = _currentCategoryEntry.LocalizationKey.GetLocalization();

            _wordsPool.EnsureCount(_currentCategoryEntry.WordEntries);
        }

        private void ReorderWordItems(WordOrderType newOrder)
        {
            _categoryStateMutator.ChangeWordOrder(_currentCategoryEntry, newOrder);

            _wordsPool.EnsureCount(_currentCategoryEntry.WordEntries);
        }
    }
}