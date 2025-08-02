using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Cysharp.Text;
using Source.Scripts.Core.Repositories;
using UnityEngine;

namespace Source.Scripts.Data.Repositories
{
    internal sealed class IdHandler<TEntry> : IIdHandler<TEntry>, IDisposable
    {
        private readonly PersistentReactiveProperty<int> _currentMaxId
            = new(ZString.Concat(PersistentKeys.CurrentMaxIdKey, typeof(TEntry).Name));

        public Dictionary<int, TEntry> GenerateWithIds(List<TEntry> entries)
        {
            var entriesWithIds = new Dictionary<int, TEntry>();

            foreach (var entry in entries)
            {
                entriesWithIds[_currentMaxId.Value] = entry;
                _currentMaxId.Value++;
            }

            return entriesWithIds;
        }

        public Dictionary<int, TEntry> GenerateWithDefaultIds<TDefaultEntry>(List<TDefaultEntry> defaultEntries)
            where TDefaultEntry : class, IDefaultEntry<TEntry>, new()
        {
            var entriesWithIds = new Dictionary<int, TEntry>();

            foreach (var defaultEntry in defaultEntries)
            {
                if (Validate(entriesWithIds, defaultEntry) is false)
                    continue;

                entriesWithIds[defaultEntry.DefaultId] = defaultEntry.Entry;
            }

            return entriesWithIds;
        }

        private bool Validate(Dictionary<int, TEntry> entries, IDefaultEntry<TEntry> currentEntry)
        {
            if (entries.ContainsKey(currentEntry.DefaultId))
            {
                Debug.LogError("[IdHandler::Validate] " +
                               $"Encountered duplicate id: {currentEntry.DefaultId}." +
                               $"skipping entry: {currentEntry.Entry}." +
                               $"For type {typeof(TEntry).Name}");
                return false;
            }

            if (currentEntry.DefaultId < 0)
                return true;

            Debug.LogError("[IdHandler::Validate] " +
                           $"Encountered positive id: {currentEntry.DefaultId} " +
                           "for generation with default ids which is prohibited, " +
                           $"skipping entry: {currentEntry.Entry}." +
                           $"For type {typeof(TEntry).Name}");
            return false;
        }

        public void Dispose()
        {
            _currentMaxId?.Dispose();
        }
    }
}