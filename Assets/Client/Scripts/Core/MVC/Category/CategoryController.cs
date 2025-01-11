using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.CategoryEntity;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.MVC.Base;

namespace Client.Scripts.MVC.Categories
{
    internal class CategoryController : ControllerBase<UserCategoryEntity, UserCategoryEntryContent, CategoryModel>
    {
        public CategoryController(IEntityController entityController, IView<CategoryModel> view)
            : base(entityController, view) { }

        internal async Task<bool> CreateCategory(string title, string description)
        {
            var content = new UserCategoryEntryContent
            {
                Title = title,
                Description = description
            };

            return await CreateEntry(content);
        }

        protected override CategoryModel CreateModel(EntryData<UserCategoryEntryContent> data)
            => new(data);
    }
}