using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Main.UI.Shared;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.CategorySearch
{
    internal sealed class CategorySearch : SearchBase<CategoryEntry>
    {
        protected override Dictionary<int, CategoryEntry> SearchResults =>
            _categoriesRepository.CategoryEntries.CurrentValue;

        private ICategoriesRepository _categoriesRepository;

        [Inject]
        internal void Inject(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
    }
}