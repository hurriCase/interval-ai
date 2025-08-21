using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
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

        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private ICategoryStateMutator _categoryStateMutator;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private readonly ReactiveProperty<WordOrderType> _wordReviewSourceType = new(WordOrderType.Default);
        private readonly Dictionary<WordEntry, WordItem> _createdWordItems = new();
        private CategoryEntry _currentCategoryEntry;
        private EnumSelectionService<WordOrderType> _enumSelectionService;
        private int _previousCategoryId;

        internal void SetParameters(CategoryEntry categoryEntry)
        {
            _currentCategoryEntry = categoryEntry;

            _wordReviewSourceType.Value = _currentCategoryEntry.WordOrderType;

            UpdateView();

            _previousCategoryId = _currentCategoryEntry.Id;
        }

        internal override void Init()
        {
            _deleteButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.RemoveCategory());

            _resetProgressButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self
                    => self._categoryStateMutator.ResetWordsProgress(self._currentCategoryEntry));

            _categoryNameText.CurrentTextSubjectObservable
                .SubscribeAndRegister(this, static (newName, self)
                    => self._categoryStateMutator.ChangeCategoryName(self._currentCategoryEntry, newName));

            _wordReviewSourceType
                .Where(this, static (_, self) => self._currentCategoryEntry != null)
                .SubscribeAndRegister(this, static (newOrder, self) => self.ReorderWordItems(newOrder));

            _wordOrderSelectionItem.Init(_wordReviewSourceType);
        }

        private void RemoveCategory()
        {
            _categoriesRepository.RemoveCategory(_currentCategoryEntry);

            Hide();
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