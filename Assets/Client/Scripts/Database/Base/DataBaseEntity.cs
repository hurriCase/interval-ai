using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Database
{
    internal abstract class DataBaseEntity<TData> where TData : class, new()
    {
        protected IDBController dbController;
        protected string userId;
        
        protected Dictionary<string, EntityData<TData>> Entities { get; set; } = new();

        protected virtual string GetPath(string userId) => string.Empty;

        public virtual async Task Init(IDBController dbController, string userId)
        {
            this.dbController = dbController;
            this.userId = userId;

            await LoadEntity();
        }

        internal virtual async Task<EntityData<TData>> CreateEntity(TData data)
        {
            var entityData = new EntityData<TData>
            {
                Data = data
            };

            Entities[entityData.Id] = entityData;
            await WriteEntity(entityData);

            return entityData;
        }

        protected virtual async Task WriteEntity(EntityData<TData> entityData)
        {
            entityData.UpdatedAt = DateTime.UtcNow;
            await dbController.WriteData(GetPath(userId), Entities);
        }

        protected virtual async Task LoadEntity()
        {
            try
            {
                var loadedEntities = await dbController.ReadData<Dictionary<string, EntityData<TData>>>(GetPath(userId));
                if (loadedEntities != null)
                {
                    Entities = loadedEntities;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading entities: {e.Message}");
            }
        }
    }
    
    internal class EntityData<T> where T : class
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public T Data { get; set; }
    }
    
    internal interface IInitializable
    {
        Task Init(IDBController dbController, string userId);
    }
}