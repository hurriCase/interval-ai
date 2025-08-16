using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
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
    internal sealed class CategoryPopUp : ParameterizedPopUpBase<CategoryEntry>
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

        private readonly ReactiveProperty<WordOrderType> _wordReviewSourceType = new(WordOrderType.Default);

        private readonly Dictionary<WordEntry, WordItem> _createdWordItems = new();

        private int _previousCategoryId;

        internal override void Init()
        {
            _deleteButton.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.RemoveCategory())
                .RegisterTo(destroyCancellationToken);

            _resetProgressButton.OnClickAsObservable()
                .Subscribe(this, static (_, self)
                    => self._categoryStateMutator.ResetWordsProgress(self.Parameters))
                .RegisterTo(destroyCancellationToken);

            _categoryNameText.CurrentTextSubjectObservable
                .Subscribe(this, (newName, self)
                    => self._categoryStateMutator.ChangeCategoryName(self.Parameters, newName))
                .RegisterTo(destroyCancellationToken);

            _wordReviewSourceType
                .Where(this, static (_, self) => self.Parameters != null)
                .Subscribe(this, static (newOrder, self) => self.ReorderWordItems(newOrder))
                .RegisterTo(destroyCancellationToken);

            _wordOrderSelectionItem.Init(_wordReviewSourceType);
        }

        private void RemoveCategory()
        {
            _categoriesRepository.RemoveCategory(Parameters);

            Hide();
        }

        internal override void Show()
        {
            _wordReviewSourceType.Value = Parameters.WordOrderType;

            UpdateView();

            base.Show();

            _previousCategoryId = Parameters.Id;
        }

        private void UpdateView()
        {
            _categoryNameText.text = Parameters.LocalizationKey.GetLocalization();

            CreateWords();
        }

        private void ReorderWordItems(WordOrderType newOrder)
        {
            _categoryStateMutator.ChangeWordOrder(Parameters, newOrder);

            for (var i = 0; i < Parameters.WordEntries.Count; i++)
            {
                var wordEntry = Parameters.WordEntries[i];
                if (_createdWordItems.TryGetValue(wordEntry, out var wordItem))
                    wordItem.transform.SetSiblingIndex(i + 1); // First element is header
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_wordsContainer);
        }

        private void CreateWords()
        {
            if (_previousCategoryId != Parameters.Id)
                foreach (var createdWordItem in _createdWordItems)
                    createdWordItem.Value.SetActive(false);

            foreach (var wordEntry in Parameters.WordEntries)
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