using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Selection.Category
{
    internal sealed class WordCategorySelectionService : CategorySelectionServiceBase
    {
        [Inject] private IWordStateMutator _wordStateMutator;

        private WordEntry _currentWordEntry;

        public WordCategorySelectionService(
            ICategoriesRepository categoriesRepository,
            ILocalizationKeysDatabase localizationKeysDatabase)
            : base(categoriesRepository, localizationKeysDatabase) { }

        internal void UpdateWord(WordEntry wordEntry)
        {
            _currentWordEntry = wordEntry;
        }

        internal override void UpdateData()
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