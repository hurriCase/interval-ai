using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Base.Id
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

        public int GetId() => _currentMaxId.Value++;

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

                entriesWithIds[defaultEntry.Id] = defaultEntry;
            }

            return entriesWithIds;
        }

        private bool Validate(Dictionary<int, TEntry> entries, IDefaultEntry currentEntry)
        {
            if (entries.ContainsKey(currentEntry.Id))
            {
                Debug.LogError(ZString.Format("[IdHandler::Validate] Encountered duplicate " +
                                              "id: {0} " +
                                              "skipping entry: " +
                                              "{1} For type {2}",
                    currentEntry.Id, currentEntry, typeof(TEntry).Name));
                return false;
            }

            if (currentEntry.Id < 0)
                return true;

            Debug.LogError(ZString.Format("[IdHandler::Validate] Encountered positive " +
                                          "id: {0} " +
                                          "for generation with default ids which is prohibited" +
                                          "skipping entry: " +
                                          "{1} For type {2}",
                currentEntry.Id, currentEntry, typeof(TEntry).Name));
            return false;
        }

        public void Dispose()
        {
            _currentMaxId?.Dispose();
        }
    }
}