using R3;
using Source.Scripts.Core.Repositories.Base.Id;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        internal sealed class CategoryStateMutator : ICategoryStateMutator
        {
            public Observable<CategoryEntry> CategoryNameChanged => _categoryNameChanged;
            private readonly Subject<CategoryEntry> _categoryNameChanged = new();

            private readonly IIdHandler<CategoryEntry> _idHandler;

            internal CategoryStateMutator(
                IIdHandler<CategoryEntry> idHandler)
            {
                _idHandler = idHandler;
            }

            public CategoryEntry CreateCategoryEntry(string name) =>
                new()
                {
                    Id = _idHandler.GetId(),
                    LocalizationKey = name
                };

            public void ChangeCategoryName(CategoryEntry categoryEntry, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return;

                categoryEntry.LocalizationKey = name;
                _categoryNameChanged.OnNext(categoryEntry);
            }
        }
    }
}