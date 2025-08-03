using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories;
using UnityEngine;

namespace Source.Scripts.Data.Repositories
{
    internal sealed class IdHandler<TEntry> : IIdHandler<TEntry>, IDisposable
    {
        private readonly PersistentReactiveProperty<int> _currentMaxId = new();

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            await _currentMaxId.InitAsync(
                ZString.Concat(PersistentKeys.CurrentMaxIdKey, typeof(TEntry).Name),
                cancellationToken);
        }

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
            where TDefaultEntry : class, TEntry, IDefaultEntry
        {
            var entriesWithIds = new Dictionary<int, TEntry>();

            foreach (var defaultEntry in defaultEntries)
            {
                if (Validate(entriesWithIds, defaultEntry) is false)
                    continue;

                entriesWithIds[defaultEntry.DefaultId] = defaultEntry;
            }

            return entriesWithIds;
        }

        private bool Validate(Dictionary<int, TEntry> entries, IDefaultEntry currentEntry)
        {
            if (entries.ContainsKey(currentEntry.DefaultId))
            {
                Debug.LogError("[IdHandler::Validate] " +
                               $"Encountered duplicate id: {currentEntry.DefaultId}." +
                               $"skipping entry: {currentEntry}." +
                               $"For type {typeof(TEntry).Name}");
                return false;
            }

            if (currentEntry.DefaultId < 0)
                return true;

            Debug.LogError("[IdHandler::Validate] " +
                           $"Encountered positive id: {currentEntry.DefaultId} " +
                           "for generation with default ids which is prohibited, " +
                           $"skipping entry: {currentEntry}." +
                           $"For type {typeof(TEntry).Name}");
            return false;
        }

        public void Dispose()
        {
            _currentMaxId?.Dispose();
        }
    }
}