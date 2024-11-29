using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Database.Base
{
    internal abstract class DBEntityBase<TData> : Injectable, IEntity<TData> where TData : class, new()
    {
        public Dictionary<string, EntityData<TData>> Entities { get; set; } = new();

        [Inject] protected IDBController dbController;

        public virtual async Task<EntityData<TData>> CreateEntityAsync(TData data)
        {
            var entityData = new EntityData<TData>
            {
                Data = data
            };

            Entities[entityData.Id] = entityData;
            entityData.UpdatedAt = DateTime.UtcNow;
            await dbController.WriteDataAsync(GetPath(), Entities);

            return entityData;
        }

        public Task<EntityData<TData>> ReadEntityAsync() => dbController.ReadDataAsync<EntityData<TData>>(GetPath());

        public async Task<EntityData<TData>> UpdateEntityAsync(EntityData<TData> entity)
        {
            if (Entities.TryGetValue(entity.Id, out _) is false)
            {
                Debug.LogError($"[DBEntityBase::UpdateEntityAsync] Entity {entity.Id} does not exist");
                return await Task.FromResult<EntityData<TData>>(null);
            }

            Entities[entity.Id] = entity;
            entity.UpdatedAt = DateTime.UtcNow;
            await dbController.UpdateDataAsync(GetPath(), Entities);

            return entity;
        }

        public async Task<EntityData<TData>> DeleteEntityAsync(EntityData<TData> data)
        {
            if (Entities.TryGetValue(data.Id, out var entityData) is false)
            {
                Debug.LogError($"[DBEntityBase::DeleteEntityAsync] Entity {data.Id} does not exist");
                return await Task.FromResult<EntityData<TData>>(null);
            }

            entityData.UpdatedAt = DateTime.UtcNow;

            //TODO:<dmitriy.sukharev> Test
            await dbController.DeleteDataAsync(GetPath() + $"/{entityData.Id}");

            return entityData;
        }


        public virtual async Task LoadEntityAsync()
        {
            try
            {
                var loadedEntities =
                    await dbController.ReadDataAsync<Dictionary<string, EntityData<TData>>>(GetPath());
                if (loadedEntities != null)
                    Entities = loadedEntities;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[DBEntityBase::LoadEntityAsync] Error loading entities: {e.Message}");
            }
        }

        public IEnumerable<EntityData<TData>> GetAllEntities() => Entities.Values;

        protected virtual string GetPath() => string.Empty;
    }

    internal class EntityData<TData> where TData : class
    {
        internal string Id { get; } = Guid.NewGuid().ToString();
        internal DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        internal DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        internal TData Data { get; set; }
    }

    internal interface IEntity<TData> where TData : class
    {
        Task<EntityData<TData>> CreateEntityAsync(TData data);
        Task<EntityData<TData>> ReadEntityAsync();
        Task<EntityData<TData>> UpdateEntityAsync(EntityData<TData> entity);
        Task<EntityData<TData>> DeleteEntityAsync(EntityData<TData> entity);
        Task LoadEntityAsync();
    }
}