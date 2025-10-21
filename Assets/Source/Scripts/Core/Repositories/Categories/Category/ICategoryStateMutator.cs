using R3;
using Source.Scripts.Core.Repositories.Categories.Base;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal interface ICategoryStateMutator
    {
        Observable<CategoryEntry> OnCategoryNameChanged { get; }
        CategoryEntry CreateCategoryEntry(string name, AssetReferenceSprite icon);
        void ChangeCategoryName(CategoryEntry categoryEntry, string name);
        void ChangeWordOrder(CategoryEntry categoryEntry, WordOrderType newOrderType);
        void ResetWordsProgress(CategoryEntry categoryEntry);
    }
}