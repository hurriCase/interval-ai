using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Database.Base
{
    internal abstract class DataBaseEntity<TData> : IInitializable where TData : class, new()
    {
        protected Dictionary<string, EntityData<TData>> Entities { get; set; } = new();
        protected IDBController dbController;
        protected string userId;

        public virtual async Task Init(IDBController dbController, string userId)
        {
            this.dbController = dbController;
            this.userId = userId;

            await LoadEntity();
        }

        protected virtual async Task<EntityData<TData>> CreateEntity(TData data)
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
                var loadedEntities =
                    await dbController.ReadData<Dictionary<string, EntityData<TData>>>(GetPath(userId));
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

        protected virtual string GetPath(string userId) => string.Empty;
    }

    internal class EntityData<T> where T : class
    {
        internal string Id { get; set; } = Guid.NewGuid().ToString();
        internal DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        internal DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        internal T Data { get; set; }
    }

    internal interface IInitializable
    {
        Task Init(IDBController dbController, string userId);
    }
}