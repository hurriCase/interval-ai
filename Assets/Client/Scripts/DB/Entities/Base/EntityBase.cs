using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.DB.Entities.Base
{
    internal abstract class EntityBase<TContent> : Injectable, IEntity<TContent> where TContent : class, new()
    {
        [Inject] protected ICloudRepository cloudRepository;

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

                Debug.Log("[EntityBase::InitAsync] EntityBase is initialized");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[EntityBase::InitAsync] Error loading entries: {e.Message}");
            }
        }

        public virtual async Task LoadEntryAsync()
        {
            try
            {
                var loadedEntries = await cloudRepository
                    .ReadDataAsync<Dictionary<string, EntryData<TContent>>>(DataType.User, GetEntryPath());

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
                Debug.LogWarning($"[EntityBase::LoadEntryAsync] Error loading entries: {e.Message}");
            }
        }

        public virtual async Task<EntryData<TContent>> CreateEntryAsync(TContent content)
        {
            if (CheckEntityBaseInit() is false)
                return null;

            var validation = ValidateEntryContent(content);
            if (validation.IsValid is false)
            {
                Debug.LogError("[EntityBase::CreateEntryAsync] Validation failed:" +
                               $" {string.Join(", ", validation.Errors)}");
                return null;
            }

            if (IsSingleInstance && Entries.Any())
            {
                Debug.LogError(
                    $"[EntityBase::CreateEntryAsync] Cannot create multiple instances of {GetType().Name}");
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

            var validation = ValidateEntryContent(entry.Content);
            if (validation.IsValid is false)
            {
                Debug.LogError("[EntityBase::UpdateEntryAsync] Validation failed:" +
                               $" {string.Join(", ", validation.Errors)}");
                return null;
            }

            if (Entries.ContainsKey(entry.Id) is false)
            {
                Debug.LogError($"[EntityBase::UpdateEntryAsync] Entry {entry.Id} does not exist");
                return null;
            }

            if (IsSingleInstance && entry.Id != Entries.First().Key)
            {
                Debug.LogError(
                    $"[EntityBase::UpdateEntryAsync] Cannot update different instance of {GetType().Name}");
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
                Debug.LogError($"[EntityBase::DeleteEntryAsync] Entry {data.Id} does not exist");
                return null;
            }

            await cloudRepository.DeleteDataAsync(DataType.User, GetEntryPath(entryData.Id));

            return entryData;
        }

        private ValidationResult ValidateEntryContent(TContent content)
        {
            if (CheckEntityBaseInit() is false)
                return null;

            var properties = typeof(TContent).GetProperties();
            var result = new ValidationResult();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(content);
                var validationAttributes =
                    prop.GetCustomAttributes<ValidationAttributeBase>();

                foreach (var attribute in validationAttributes)
                {
                    var (isValid, error) = attribute.Validate(value);
                    if (!isValid)
                        result.Errors.Add($"{prop.Name}: {error}");
                }
            }

            return result;
        }

        private bool CheckEntityBaseInit()
        {
            if (_isInited is false)
                Debug.LogError("[EntityBase::CheckEntityBaseInit] " +
                               "EntityBase not initialized but you're trying to access");

            return _isInited;
        }

        protected string GetEntryPath(string id = null, string entityPath = null)
            => $"{entityPath ?? EntityPath}{(id is null ? "" : "/" + id)}";
    }
}