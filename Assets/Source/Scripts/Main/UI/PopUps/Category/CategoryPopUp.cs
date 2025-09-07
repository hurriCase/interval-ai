using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
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
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class CategoryPopUp : PopUpBase
    {
        [SerializeField] private InputFieldComponent _categoryNameText;

        [SerializeField] private ButtonComponent _deleteButton;
        [SerializeField] private ButtonComponent _editButton;

        [SerializeField] private ButtonComponent _resetProgressButton;

        [SerializeField] private RectTransform _wordsContainer;
        [SerializeField] private WordItem _wordItem;

        [SerializeField] private SelectionItem _wordOrderSelectionItem;

        private readonly ReactiveProperty<WordOrderType> _wordReviewSourceType = new(WordOrderType.Default);
        private readonly Dictionary<WordEntry, WordItem> _createdWordItems = new();
        private CategoryEntry _currentCategoryEntry;
        private EnumSelectionService<WordOrderType> _enumSelectionService;
        private int _previousCategoryId;

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

            _previousCategoryId = _currentCategoryEntry.Id;
        }

        internal override void Init()
        {
            _deleteButton.OnClickAsObservable().SubscribeAndRegister(this, static self =>
                self.OpenWarning(ModalLocalizationType.DeleteCategory, static self => self.RemoveCategory()));

            _resetProgressButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.OpenWarning(
                ModalLocalizationType.ResetProgress,
                static self => self._categoryStateMutator.ResetWordsProgress(self._currentCategoryEntry)));

            _categoryNameText.CurrentTextSubjectObservable
                .SubscribeAndRegister(this, static (newName, self)
                    => self._categoryStateMutator.ChangeCategoryName(self._currentCategoryEntry, newName));

            _wordReviewSourceType
                .Where(this, static (_, self) => self._currentCategoryEntry != null)
                .SubscribeAndRegister(this, static (newOrder, self) => self.ReorderWordItems(newOrder));

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

            CreateWords();
        }

        private void ReorderWordItems(WordOrderType newOrder)
        {
            _categoryStateMutator.ChangeWordOrder(_currentCategoryEntry, newOrder);

            for (var i = 0; i < _currentCategoryEntry.WordEntries.Count; i++)
            {
                var wordEntry = _currentCategoryEntry.WordEntries[i];
                if (_createdWordItems.TryGetValue(wordEntry, out var wordItem))
                    wordItem.transform.SetSiblingIndex(i + 1); // First element is header
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsContainer);
        }

        private void CreateWords()
        {
            if (_previousCategoryId != _currentCategoryEntry.Id)
                foreach (var createdWordItem in _createdWordItems)
                    createdWordItem.Value.SetActive(false);

            foreach (var wordEntry in _currentCategoryEntry.WordEntries)
                CreateWord(wordEntry);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsContainer);
        }

        private void CreateWord(WordEntry wordEntry)
        {
            if (_createdWordItems.TryGetValue(wordEntry, out var createdWord))
            {
                createdWord.SetActive(true);
                return;
            }

            createdWord = _objectResolver.Instantiate(_wordItem, _wordsContainer);
            createdWord.Init(wordEntry);
            _createdWordItems[wordEntry] = createdWord;
        }
    }
}