namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal interface ICategoryStateMutator
    {
        CategoryEntry CreateCategoryEntry(string name);
    }
}