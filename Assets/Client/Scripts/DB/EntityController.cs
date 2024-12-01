using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Client.Scripts.DB.Base;
using Client.Scripts.DB.Entities;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;
using Exception = System.Exception;

namespace Client.Scripts.DB
{
    internal class EntityController : Injectable, IEntityController
    {
        [Inject] private IDBController _dbController;

        private readonly ConcurrentDictionary<Type, object> _entities = new();
        private readonly ConcurrentDictionary<Type, object> _entityData = new();

        internal event Action<Type> OnEntityCreated;
        internal event Action<Type> OnEntityRead;
        internal event Action<Type> OnEntityUpdated;
        internal event Action<Type> OnEntityDeleted;

        public async Task InitAsync()
        {
            var entityRegistrations = new List<(Type Type, object Entity)>
            {
                (typeof(WordEntityData), new WordEntity()),
                (typeof(CategoryEntityData), new CategoryEntity()),
                (typeof(ProgressEntityData), new ProgressEntity()),
                (typeof(UserEntityData), new UserEntity())
            };

            var registrationTasks = entityRegistrations.Select(RegisterEntityAsync);
            await Task.WhenAll(registrationTasks);
        }

        private async Task RegisterEntityAsync((Type Type, object Entity) registration)
        {
            try
            {
                var loadMethod = registration.Entity.GetType()
                    .GetMethod("LoadEntityAsync", BindingFlags.Public | BindingFlags.Instance);

                if (loadMethod != null)
                    await (Task)loadMethod.Invoke(registration.Entity, null);

                _entities[registration.Type] = registration.Entity;
                _entityData[registration.Type] = Activator.CreateInstance(
                    typeof(EntityData<>).MakeGenericType(registration.Type)
                );
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to register entity {registration.Type}: {ex.Message}");
            }
        }

        public async Task<bool> ExistsAsync<TData>() where TData : class
        {
            var entity = GetEntity<TData>();
            if (entity == null)
                return false;

            var existingData = await entity.ReadEntityAsync();
            return existingData?.Data != null;
        }

        public async Task<EntityResult<TData>> CreateEntityAsync<TData>(TData data) where TData : class
        {
            try
            {
                var entity = GetEntity<TData>();
                if (entity == null)
                    return EntityResult<TData>.Failure("Entity type not registered");

                var createdEntity = await entity.CreateEntityAsync(data);
                
                OnEntityCreated?.Invoke(typeof(TData));

                return createdEntity != null
                    ? EntityResult<TData>.Success(createdEntity.Data)
                    : EntityResult<TData>.Failure("Entity creation failed");
            }
            catch (Exception ex)
            {
                return EntityResult<TData>.Failure("Error during entity creation", ex);
            }
        }

        public async Task<EntityResult<TData>> ReadEntityAsync<TData>() where TData : class
        {
            try
            {
                var entity = GetEntity<TData>();
                if (entity == null)
                    return EntityResult<TData>.Failure("Entity type not registered");

                var readEntity = await entity.ReadEntityAsync();

                OnEntityRead?.Invoke(typeof(TData));

                return readEntity != null
                    ? EntityResult<TData>.Success(readEntity.Data)
                    : EntityResult<TData>.Failure("No entities found");
            }
            catch (Exception ex)
            {
                return EntityResult<TData>.Failure("Error reading entities", ex);
            }
        }

        public async Task<EntityResult<TData>> UpdateEntityAsync<TData>(TData data) where TData : class
        {
            try
            {
                var entity = GetEntity<TData>();
                if (entity == null)
                    return EntityResult<TData>.Failure("Entity type not registered");

                var entityDataType = typeof(EntityData<TData>);
                if (Activator.CreateInstance(entityDataType) is EntityData<TData> entityData)
                {
                    entityData.Data = data;

                    var updatedEntity = await entity.UpdateEntityAsync(entityData);

                    OnEntityUpdated?.Invoke(typeof(TData));

                    return updatedEntity != null
                        ? EntityResult<TData>.Success(updatedEntity.Data)
                        : EntityResult<TData>.Failure("Entity update failed");
                }

                return EntityResult<TData>.Failure("Error during entity update");
            }
            catch (Exception ex)
            {
                return EntityResult<TData>.Failure("Error during entity update", ex);
            }
        }

        //TODO:<dmitriy.sukharev> test
        public async Task<EntityResult<TData>> DeleteEntityAsync<TData>(string id) where TData : class
        {
            try
            {
                var entity = GetEntity<TData>();
                if (entity == null)
                    return EntityResult<TData>.Failure("Entity type not registered");
                
                var entityToDelete = entity.Entities?.Values
                    .FirstOrDefault(e => e.Id == id);

                var deletedEntity = await entity.DeleteEntityAsync(entityToDelete);

                OnEntityDeleted?.Invoke(typeof(TData));

                return deletedEntity != null
                    ? EntityResult<TData>.Success(deletedEntity.Data)
                    : EntityResult<TData>.Failure("Entity deletion failed");
            }
            catch (Exception ex)
            {
                return EntityResult<TData>.Failure("Error during entity deletion", ex);
            }
        }

        public IEnumerable<TData> FindEntitiesAsync<TData>(Func<TData, bool> predicate)
            where TData : class
        {
            var entity = GetEntity<TData>();
            if (entity == null)
                return Enumerable.Empty<TData>();
            
            return entity.Entities?
                       .Values
                       .Select(e => e.Data)
                       .Where(predicate)
                   ?? Enumerable.Empty<TData>();
        }

        private IEntity<TData> GetEntity<TData>() where TData : class
        {
            return _entities.TryGetValue(typeof(TData), out var entityObj)
                ? entityObj as IEntity<TData>
                : null;
        }
    }
}