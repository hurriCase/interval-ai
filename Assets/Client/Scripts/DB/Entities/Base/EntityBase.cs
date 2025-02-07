using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.Entities.Base.Validation;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;

namespace Client.Scripts.DB.Entities.Base
{
    internal abstract class EntityBase<TContent> : Injectable, IEntity<TContent>
        where TContent : class, new()
    {
        [Inject] protected ICloudRepository cloudRepository;
        [Inject] protected IEntityValidationController entityValidationController;

        public ConcurrentDictionary<string, EntryData<TContent>> Entries { get; set; } = new();
        protected abstract string EntityPath { get; }

        private bool IsSingleInstance => GetType().GetCustomAttribute<SingleInstanceEntryAttribute>() != null;
        private bool _isInited;

        public virtual async Task InitAsync()
        {
            try
            {
                if (_isInited)
                    return;

                await LoadEntryAsync();

                _isInited = true;

                Debug.Log($"[{GetType().Name}::InitAsync] EntityBase is initialized");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[{GetType().Name}::InitAsync] Error loading entries: {e.Message}");
            }
        }

        public virtual async Task LoadEntryAsync()
        {
            try
            {
                var loadedEntries = await cloudRepository
                    .LoadDataAsync<Dictionary<string, EntryData<TContent>>>(DataType.User, GetEntryPath());

                if (loadedEntries != null)
                {
                    Entries.Clear();

                    foreach (var (id, entryData) in loadedEntries)
                    {
                        Entries[id] = entryData;
                        cloudRepository.ListenForValueChanged<EntryData<TContent>>(
                            DataType.User,
                            GetEntryPath(entryData.Id),
                            _ => entryData.UpdatedAt = DateTime.Now
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[{GetType().Name}::LoadEntryAsync] Error loading entries: {e.Message}");
            }
        }

        public virtual async Task<EntryData<TContent>> CreateEntryAsync(TContent content)
        {
            if (CheckEntityBaseInit() is false)
                return null;

            var validation = entityValidationController.ValidateEntityContent(GetType().Name, content);
            if (validation.IsValid is false)
            {
                Debug.LogError($"[{GetType().Name}::CreateEntryAsync] Validation failed:" +
                               $" {string.Join(", ", validation.Errors)}");
                return null;
            }

            if (IsSingleInstance && Entries.Any())
            {
                Debug.LogError(
                    $"[{GetType().Name}::CreateEntryAsync] Cannot create multiple instances of {GetType().Name}");
                return null;
            }

            var entryData = new EntryData<TContent>
            {
                Content = content,
                UpdatedAt = DateTime.UtcNow
            };

            Entries[entryData.Id] = entryData;
            await cloudRepository.WriteDataAsync(DataType.User, GetEntryPath(entryData.Id), entryData);

            return entryData;
        }

        public async Task<EntryData<TContent>> ReadEntryAsync(string id) =>
            CheckEntityBaseInit() is false
                ? null
                : await cloudRepository.ReadDataAsync<EntryData<TContent>>(DataType.User, GetEntryPath(id));

        public async Task<EntryData<TContent>> UpdateEntryAsync(EntryData<TContent> entry)
        {
            if (CheckEntityBaseInit() is false)
                return null;

            var validation = entityValidationController.ValidateEntityContent(GetType().Name, entry.Content);
            if (validation.IsValid is false)
            {
                Debug.LogError($"[{GetType().Name}::UpdateEntryAsync] Validation failed:" +
                               $" {string.Join(", ", validation.Errors)}");
                return null;
            }

            if (Entries.ContainsKey(entry.Id) is false)
            {
                Debug.LogError($"[{GetType().Name}::UpdateEntryAsync] Entry {entry.Id} does not exist");
                return null;
            }

            if (IsSingleInstance && entry.Id != Entries.First().Key)
            {
                Debug.LogError(
                    $"[{GetType().Name}::UpdateEntryAsync] Cannot update different instance of {GetType().Name}");
                return null;
            }

            entry.UpdatedAt = DateTime.UtcNow;
            Entries[entry.Id] = entry;
            await cloudRepository.UpdateDataAsync(DataType.User, GetEntryPath(entry.Id), entry);

            return entry;
        }

        public async Task<EntryData<TContent>> DeleteEntryAsync(EntryData<TContent> data)
        {
            if (CheckEntityBaseInit() is false)
                return null;

            if (Entries.TryRemove(data.Id, out var entryData) is false)
            {
                Debug.LogError($"[{GetType().Name}::DeleteEntryAsync] Entry {data.Id} does not exist");
                return null;
            }

            await cloudRepository.DeleteDataAsync(DataType.User, GetEntryPath(entryData.Id));

            return entryData;
        }

        private bool CheckEntityBaseInit()
        {
            if (_isInited is false)
                Debug.LogError($"[{GetType().Name}::CheckEntityBaseInit] " +
                               $"{GetType().Name} not initialized but you're trying to access");

            return _isInited;
        }

        protected string GetEntryPath(string id = null, string entityPath = null)
            => $"{entityPath ?? EntityPath}{(id is null ? "" : "/" + id)}";
    }
}