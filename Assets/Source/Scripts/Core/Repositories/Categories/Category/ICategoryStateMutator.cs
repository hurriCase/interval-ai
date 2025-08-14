using R3;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal interface ICategoryStateMutator
    {
        Observable<CategoryEntry> CategoryNameChanged { get; }
        CategoryEntry CreateCategoryEntry(string name);
        void ChangeCategoryName(CategoryEntry categoryEntry, string name);
    }
}