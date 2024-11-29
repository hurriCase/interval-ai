using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Database.Base;
using Client.Scripts.Database.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

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
            await entity.LoadEntityAsync();
            _entities[typeof(TEntity)] = entity;
            _entityData[typeof(TEntity)] = data;
        }

        public IEntity<TData> GetEntity<TEntity, TData>()
            where TEntity : IEntity<TData>
            where TData : class
        {
            if (_entities.TryGetValue(typeof(TData), out var entity))
                return (IEntity<TData>)entity;

            Debug.LogWarning($"[EntityController::GetEntity] Entity of type {typeof(TData)} not found");
            return null;
        }

        public EntityData<TData> GetEntityData<TData>()
            where TData : class
        {
            if (_entityData.TryGetValue(typeof(TData), out var entityData))
                return (EntityData<TData>)entityData;

            Debug.LogWarning($"[EntityController::GetEntityData] EntityData of type {typeof(TData)} not found");
            return null;
        }

        public async Task<EntityData<TData>> CreateEntity<TEntity, TData>(TEntity entity,
            TData entityData)
            where TData : class, new()
            where TEntity : IEntity<TData>
        {
            OnCreatedEntity?.Invoke();
            return await entity.CreateEntityAsync(entityData);
        }

        public async Task<EntityData<TData>> ReadEntity<TEntity, TData>(IEntity<TData> entity)
            where TEntity : class
            where TData : class
        {
            OnReadEntity?.Invoke();
            return await entity.ReadEntityAsync();
        }

        public async Task<EntityData<TData>> UpdateEntity<TData>(IEntity<TData> entity,
            EntityData<TData> entityData)
            where TData : class
        {
            OnUpdatedEntity?.Invoke();
            return await entity.UpdateEntityAsync(entityData);
        }

        public async Task<EntityData<TData>> DeleteEntity<TData>(IEntity<TData> entity,
            EntityData<TData> entityData)
            where TData : class
        {
            OnDeletedEntity?.Invoke();
            return await entity.DeleteEntityAsync(entityData);
        }
    }
}