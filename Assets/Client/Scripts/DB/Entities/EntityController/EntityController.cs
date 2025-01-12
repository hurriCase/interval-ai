using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.Patterns.DI.Base;
using UnityEngine;
using Exception = System.Exception;

namespace Client.Scripts.DB.Entities.EntityController
{
    internal sealed class EntityController : Injectable, IEntityController
    {
        [Inject] private ICloudRepository _cloudRepository;

        private ConcurrentDictionary<Type, object> _entities = new();
        private bool _isInited;

        public event Action<Type, object> OnEntryCreated;
        public event Action<Type, object> OnEntryRead;
        public event Action<Type, object> OnEntryUpdated;
        public event Action<Type, object> OnEntryDeleted;

        public async Task InitAsync()
        {
            if (_isInited)
                return;

            try
            {
                _entities = await EntityFactory.CreateEntitiesAsync();

                _isInited = true;

                Debug.Log("[EntityController::InitAsync]] EntityController is initialized");
            }
            catch (Exception e)
            {
                Debug.LogError($"[FireBaseDB::InitAsync] Failed to initialize EntityController: {e.Message}");
            }
        }

        public async Task<EntityResult<TContent>> CreateEntryAsync<TEntity, TContent>(TContent content)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            try
            {
                var entity = GetEntity<TEntity, TContent>();
                if (entity == null)
                    return EntityResult<TContent>.Failure("[EntityController::DeleteEntryAsync] " +
                                                          "Entity type not registered");

                var createdEntry = await entity.CreateEntryAsync(content);

                OnEntryCreated?.Invoke(typeof(TEntity), createdEntry.Content);

                return createdEntry != null
                    ? EntityResult<TContent>.Success(createdEntry)
                    : EntityResult<TContent>.Failure("[EntityController::CreateEntryAsync] " +
                                                     "Entry creation failed");
            }
            catch (Exception ex)
            {
                return EntityResult<TContent>.Failure("[EntityController::CreateEntryAsync] " +
                                                      "Error during entry creation", ex);
            }
        }

        public async Task<EntityResult<TContent>> ReadEntryAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            try
            {
                var entity = GetEntity<TEntity, TContent>();
                if (entity == null)
                    return EntityResult<TContent>.Failure("[EntityController::ReadEntryAsync] " +
                                                          "Entity type not registered");

                var readEntry = await entity.ReadEntryAsync(id);

                OnEntryRead?.Invoke(typeof(TEntity), readEntry.Content);

                return readEntry != null
                    ? EntityResult<TContent>.Success(readEntry)
                    : EntityResult<TContent>.Failure("[EntityController::ReadEntryAsync] No entry found");
            }
            catch (Exception ex)
            {
                return EntityResult<TContent>.Failure("[EntityController::ReadEntryAsync] " +
                                                      "Error reading entry", ex);
            }
        }

        public async Task<EntityResult<TContent>> UpdateEntryAsync<TEntity, TContent>
            (EntryData<TContent> entry, TContent content)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            try
            {
                var entity = GetEntity<TEntity, TContent>();
                if (entity == null)
                    return EntityResult<TContent>.Failure("[EntityController::ReadEntryAsync] " +
                                                          "Entity type not registered");

                entry.Content = content;

                var updatedEntry = await entity.UpdateEntryAsync(entry);

                OnEntryUpdated?.Invoke(typeof(TEntity), updatedEntry.Content);

                return updatedEntry != null
                    ? EntityResult<TContent>.Success(updatedEntry)
                    : EntityResult<TContent>.Failure("[EntityController::UpdateEntryAsync] " +
                                                     "Entry update failed");
            }
            catch (Exception ex)
            {
                return EntityResult<TContent>.Failure("[EntityController::UpdateEntryAsync] " +
                                                      "Error during entry update", ex);
            }
        }

        public async Task<EntityResult<TContent>> DeleteEntryAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            try
            {
                var entity = GetEntity<TEntity, TContent>();
                if (entity == null)
                    return EntityResult<TContent>.Failure("[EntityController::DeleteEntryAsync] " +
                                                          "Entity type not registered");

                var entryToDelete = FindEntryById<TEntity, TContent>(id);

                var deletedEntity = await entity.DeleteEntryAsync(entryToDelete);

                OnEntryDeleted?.Invoke(typeof(TEntity), deletedEntity.Content);

                return deletedEntity != null
                    ? EntityResult<TContent>.Success(deletedEntity)
                    : EntityResult<TContent>.Failure("[EntityController::DeleteEntryAsync] " +
                                                     "Entry deletion failed");
            }
            catch (Exception ex)
            {
                return EntityResult<TContent>.Failure("[EntityController::DeleteEntryAsync] " +
                                                      "Error during entry deletion", ex);
            }
        }

        public EntryData<TContent>[] FindEntries<TEntity, TContent>
            (Func<EntryData<TContent>, bool> predicate)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            var entity = GetEntity<TEntity, TContent>();

            return entity?.Entries?.Values
                .Where(predicate)
                .ToArray();
        }

        public EntryData<TContent> FindEntryById<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            var entity = GetEntity<TEntity, TContent>();

            return entity?.Entries?.GetValueOrDefault(id);
        }

        public async Task<bool> ExistsAsync<TEntity, TContent>(string id)
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return false;

            var entity = GetEntity<TEntity, TContent>();
            if (entity == null)
                return false;

            var existingData = await entity.ReadEntryAsync(id);
            return existingData?.Content != null;
        }

        private IEntity<TContent> GetEntity<TEntity, TContent>()
            where TEntity : IEntity<TContent>
            where TContent : class
        {
            if (CheckEntityControllerInit() is false)
                return null;

            return _entities.TryGetValue(typeof(TEntity), out var entityObj)
                ? entityObj as IEntity<TContent>
                : null;
        }

        private bool CheckEntityControllerInit()
        {
            if (_isInited is false)
                Debug.LogError("[EntityController::CheckEntityControllerInit] " +
                               "EntityController not initialized but you're trying to access");

            return _isInited;
        }
    }
}