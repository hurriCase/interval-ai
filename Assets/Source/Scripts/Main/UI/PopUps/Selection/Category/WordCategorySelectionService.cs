using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Main.UI.PopUps.Selection.Category
{
    internal sealed class WordCategorySelectionService : CategorySelectionServiceBase
    {
        private WordEntry _currentWordEntry;

        private readonly IWordStateMutator _wordStateMutator;

        internal WordCategorySelectionService(
            ICategoriesRepository categoriesRepository,
            IWordStateMutator wordStateMutator)
            : base(categoriesRepository)
        {
            _wordStateMutator = wordStateMutator;
        }

        internal void UpdateWord(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;
        }

        internal void UpdateData()
        {
            var currentSelectionValues = categoriesRepository.CategoryEntries.CurrentValue;
            SelectionValues = currentSelectionValues.Keys.ToArray();
            selectedValues.Value = _currentWordEntry.CategoryIds;
            currentCategories = currentSelectionValues;
        }

        protected override void SetCategories(List<int> selectedValues)
        {
            _wordStateMutator.SetCategories(_currentWordEntry, selectedValues);
        }
    }
}