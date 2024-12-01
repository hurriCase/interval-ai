using System.Linq;
using Client.Scripts.DB;
using Client.Scripts.DB.Base;
using Client.Scripts.DB.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.UI.Base;
using UnityEngine;

namespace Client.Scripts.UI.MainWindows
{
    internal sealed class CategoriesWindow : WindowBase
    {
        [Inject] private IEntityController _entityController;

        //TODO:<dmitriy.sukharev> Is made for test
        private void Start()
        {
            _entityController.CreateEntityAsync(
                new CategoryEntityData
                {
                    Title = "Test 1",
                    Description = null,
                    Words = null,
                    IsDefault = false
                });

            _entityController.ReadEntityAsync<CategoryEntityData>();

            var id = _entityController.FindEntitiesAsync<EntityData<CategoryEntityData>>(
                entity => entity.Data.IsDefault)?.First().Id;

            _entityController.UpdateEntityAsync(
                new CategoryEntityData
                {
                    Title = "Test 2",
                    Description = null,
                    Words = null,
                    IsDefault = false
                });

            _entityController.DeleteEntityAsync<CategoryEntityData>(id);
        }
    }
}