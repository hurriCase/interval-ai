using Client.Scripts.Core.MVC.Base;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.UserCategory;

namespace Client.Scripts.Core.MVC.Category
{
    internal sealed class CategoryModel : ModelBase<UserCategoryEntryContent>
    {
        internal string Title => Data.Content.Title;
        internal string Description => Data.Content.Description;
        internal int WordCount => Data.Content.Words?.Count ?? 0;

        internal CategoryModel(EntryData<UserCategoryEntryContent> data) : base(data) { }
    }
}