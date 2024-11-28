using System.Threading.Tasks;
using Client.Scripts.Database;
using Client.Scripts.Database.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;
using IInitializable = Client.Scripts.Database.Base.IInitializable;

namespace Client.Scripts.Database.Controllers
{
    internal class VocabularyEntityController : Injectable, IVocabularyEntityController
    {
        [Inject] private IDBController _dbController;
        private IInitializable[] _entities;

        public async Task Init()
        {
            _entities = new IInitializable[]
            {
                new WordEntity(),
                new CategoryEntity(),
                new ProgressEntity()
            };

            foreach (var entity in _entities)
                await entity.Init(_dbController, _dbController.UserID);
        }
    }

    public interface IVocabularyEntityController
    {
        public Task Init();
    }
}