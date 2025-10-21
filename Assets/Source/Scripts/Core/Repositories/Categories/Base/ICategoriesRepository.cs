using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Repositories.Categories.Category;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.Categories.Base
{
    internal interface ICategoriesRepository
    {
        ReadOnlyReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries { get; }
        Observable<CategoryEntry> OnCategoryAdded { get; }
        Observable<CategoryEntry> OnCategoryRemoved { get; }
        CategoryEntry CreateCategory(string name, AssetReferenceSprite icon);
        void RemoveCategory(CategoryEntry categoryEntry);
        string GetCategoryName(int categoryId);
        bool TrySelectRandomCategory();
        Dictionary<int, CategoryEntry> GetUnselectedCategories();
        void SetSelectedCategories(List<int> categoryIds);
    }
}