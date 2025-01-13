using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.GlobalCategory;
using Client.Scripts.DB.Entities.Progress;
using Client.Scripts.DB.Entities.User;
using Client.Scripts.DB.Entities.UserCategory;
using Client.Scripts.DB.Entities.Word;

namespace Client.Scripts.DB.Entities.EntityController
{
    internal static class EntityFactory
    {
        private static readonly ConcurrentDictionary<Type, object> _entities = new();

        internal static async Task<ConcurrentDictionary<Type, object>> CreateEntitiesAsync()
        {
            await AddEntity<UserCategoryEntity, UserCategoryEntryContent>();
            await AddEntity<GlobalCategoryEntity, GlobalCategoryEntryContent>();
            await AddEntity<UserEntity, UserEntryContent>();
            await AddEntity<WordEntity, WordEntryContent>();
            await AddEntity<ProgressEntity, ProgressEntryContent>();

            return _entities;
        }

        private static async Task AddEntity<TEntity, TContent>()
            where TEntity : IEntity<TContent>
            where TContent : class, new()
        {
            var entity = Activator.CreateInstance<TEntity>();
            await entity.InitAsync();
            _entities[typeof(TEntity)] = entity;
        }
    }
}