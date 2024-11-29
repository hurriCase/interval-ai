using System;
using System.Collections.Generic;
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
        private readonly Dictionary<Type, object> _entities = new();
        private readonly Dictionary<Type, object> _entityData = new();

        public event Action OnCreatedEntity;
        public event Action OnReadEntity;
        public event Action OnUpdatedEntity;
        public event Action OnDeletedEntity;

        public async Task InitAsync()
        {
            await RegisterEntityAsync(new WordEntity(), new EntityData<WordEntityData>());
            await RegisterEntityAsync(new CategoryEntity(), new EntityData<CategoryEntityData>());
            await RegisterEntityAsync(new ProgressEntity(), new EntityData<ProgressEntityData>());
            await RegisterEntityAsync(new UserEntity(), new EntityData<UserEntityData>());
        }

        private async Task RegisterEntityAsync<TEntity>(IEntity<TEntity> entity, EntityData<TEntity> data)
            where TEntity : class
        {
            await entity.InitAsync(_dbController, _dbController.UserID);
            _entities[typeof(TEntity)] = entity;
            _entityData[typeof(TEntity)] = data;
        }

        public IEntity<TSpecificData> GetEntity<TEntity, TSpecificData>()
            where TEntity : IEntity<TSpecificData>
            where TSpecificData : class
        {
            if (_entities.TryGetValue(typeof(TSpecificData), out var entity))
                return (IEntity<TSpecificData>)entity;

            throw new InvalidOperationException($"Entity of type {typeof(TSpecificData)} not found");
        }

        public EntityData<TSpecificData> GetEntityData<TSpecificData>()
            where TSpecificData : class
        {
            if (_entityData.TryGetValue(typeof(TSpecificData), out var entityData))
                return (EntityData<TSpecificData>)entityData;

            throw new InvalidOperationException($"EntityData of type {typeof(TSpecificData)} not found");
        }

        public async Task<EntityData<TSpecificData>> CreateEntity<TEntity, TSpecificData>(TEntity entity,
            TSpecificData entityData)
            where TSpecificData : class, new()
            where TEntity : IEntity<TSpecificData>
        {
            OnCreatedEntity?.Invoke();
            return entityData != null
                ? await entity.CreateEntityAsync(entityData)
                : null;
        }

        public async Task<EntityData<TSpecificData>> ReadEntity<TEntity, TSpecificData>(IEntity<TSpecificData> entity)
            where TEntity : class
            where TSpecificData : class
        {
            OnReadEntity?.Invoke();
            return await entity.ReadEntityAsync();
        }

        public async Task<EntityData<TSpecificData>> UpdateEntity<TSpecificData>(IEntity<TSpecificData> entity,
            EntityData<TSpecificData> entityData)
            where TSpecificData : class
        {
            OnUpdatedEntity?.Invoke();
            return await entity.UpdateEntityAsync(entityData);
        }

        public async Task<EntityData<TSpecificData>> DeleteEntity<TSpecificData>(IEntity<TSpecificData> entity,
            EntityData<TSpecificData> entityData)
            where TSpecificData : class
        {
            OnDeletedEntity?.Invoke();
            return await entity.DeleteEntityAsync(entityData);
        }
    }
}