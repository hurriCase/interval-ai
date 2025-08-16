using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.Category
{
    internal sealed class CategoryPopUp : ParameterizedPopUpBase<CategoryEntry>
    {
        [SerializeField] private InputFieldComponent _categoryNameText;

        [SerializeField] private ButtonComponent _deleteButton;
        [SerializeField] private ButtonComponent _editButton;

        [SerializeField] private RectTransform _wordsContainer;
        [SerializeField] private WordItem _wordItem;

        [Inject] private ICategoriesRepository _categoriesRepository;
        [Inject] private ICategoryStateMutator _categoryStateMutator;
        [Inject] private IObjectResolver _objectResolver;

        private readonly Dictionary<WordEntry, WordItem> _createdWordItems = new();
        private readonly Queue<WordItem> _cachedWordItems = new();

        internal override void Init()
        {
            _deleteButton.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.RemoveCategory())
                .RegisterTo(destroyCancellationToken);

            _categoryNameText.CurrentTextSubjectObservable
                .Subscribe(this, (newName, self)
                    => self._categoryStateMutator.ChangeCategoryName(self.Parameters, newName))
                .RegisterTo(destroyCancellationToken);
        }

        private void RemoveCategory()
        {
            _categoriesRepository.RemoveCategory(Parameters);
            Hide();
        }

        internal override void Show()
        {
            UpdateView();

            base.Show();
        }

        private void UpdateView()
        {
            _categoryNameText.text = Parameters.LocalizationKey.GetLocalization();

            CreateWords();
        }

        private void CreateWords()
        {
            if (Parameters?.WordEntries == null)
                return;

            foreach (var wordEntry in Parameters.WordEntries)
                CreateWord(wordEntry);
        }

        private void CreateWord(WordEntry wordEntry)
        {
            if (_createdWordItems.TryGetValue(wordEntry, out var createdWord) is false)
            {
                if (_cachedWordItems.TryDequeue(out var cachedWord))
                {
                    cachedWord.SetActive(true);
                    cachedWord.transform.SetParent(_wordsContainer);
                    createdWord = cachedWord;
                }
                else
                {
                    createdWord = _objectResolver.Instantiate(_wordItem, _wordsContainer);
                    _cachedWordItems.Enqueue(createdWord);
                }
            }

            createdWord.Init(wordEntry);
            _createdWordItems[wordEntry] = createdWord;
        }
    }
}