namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        internal sealed class CategoryStateMutator : ICategoryStateMutator
        {
            public CategoryEntry CreateCategoryEntry(string name)
                => new() { LocalizationKey = name };
        }
    }
}