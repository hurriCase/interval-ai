using System.Threading.Tasks;
using Client.Scripts.Database.Base;
using Client.Scripts.Database.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.Database
{
    internal class EntityController : Injectable, IEntityController
    {
        [Inject] private IDBController _dbController;
        private IInitializable[] _entities;

        public async Task Init()
        {
            _entities = new IInitializable[]
            {
                new WordEntity(),
                new CategoryEntity(),
                new ProgressEntity(),
                new UserEntity()
            };

            foreach (var entity in _entities)
                await entity.Init(_dbController, _dbController.UserID);
        }
    }

    public interface IEntityController
    {
        public Task Init();
    }
}