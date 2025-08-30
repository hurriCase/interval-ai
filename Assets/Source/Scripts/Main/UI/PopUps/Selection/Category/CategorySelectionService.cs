using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Categories.Base;

namespace Source.Scripts.Main.UI.PopUps.Selection.Category
{
    internal sealed class CategorySelectionService : CategorySelectionServiceBase
    {
        internal CategorySelectionService(
            ICategoriesRepository categoriesRepository,
            ILocalizationKeysDatabase localizationKeysDatabase)
            : base(categoriesRepository, localizationKeysDatabase) { }

        internal override void UpdateData()
        {
            var currentSelectionValues = categoriesRepository.GetUnselectedCategories();
            SelectionValues = currentSelectionValues.Keys.ToArray();
            selectedValues.Value = new List<int>();
            currentCategories = currentSelectionValues;
        }

        protected override void SetCategories(List<int> selectedValues)
        {
            categoriesRepository.SetSelectedCategories(selectedValues);
        }
    }
}