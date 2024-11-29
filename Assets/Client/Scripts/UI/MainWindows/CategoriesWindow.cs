using System.Collections.Generic;
using Client.Scripts.Database;
using Client.Scripts.Database.Base;
using Client.Scripts.Database.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.UI.Base;

namespace Client.Scripts.UI.MainWindows
{
    internal sealed class CategoriesWindow : WindowBase
    {
        [Inject] private IEntityController _entityController;

        //TODO:<dmitriy.sukharev> Made for test
        private void Start()
        {
            _entityController.CreateEntity(new CategoryEntity(), new CategoryEntityData
            {
                Title = "Test",
                Description = "Test",
                Words = new List<EntityData<WordEntityData>>(),
                IsDefault = true
            });

            _entityController.ReadEntity<CategoryEntity, CategoryEntityData>(
                _entityController.GetEntity<CategoryEntity, CategoryEntityData>());

            _entityController.UpdateEntity(
                _entityController.GetEntity<CategoryEntity, CategoryEntityData>(),
                new EntityData<CategoryEntityData>
                {
                    CreatedAt = default,
                    UpdatedAt = default,
                    Data = null
                });

            _entityController.DeleteEntity(
                _entityController.GetEntity<CategoryEntity, CategoryEntityData>(),
                _entityController.GetEntityData<CategoryEntityData>());
        }
    }
}