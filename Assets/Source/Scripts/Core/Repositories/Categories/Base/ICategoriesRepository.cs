using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Repositories.Categories.Category;

namespace Source.Scripts.Core.Repositories.Categories.Base
{
    internal interface ICategoriesRepository
    {
        ReadOnlyReactiveProperty<Dictionary<int, Category.CategoryEntry>> CategoryEntries { get; }
        CategoryEntry CreateCategory(string name);
    }
}