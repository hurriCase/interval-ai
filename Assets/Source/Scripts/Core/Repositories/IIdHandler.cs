using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Repositories
{
    internal interface IIdHandler<TEntry>
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        Dictionary<int, TEntry> GenerateWithIds(List<TEntry> entries);

        public Dictionary<int, TEntry> GenerateWithDefaultIds<TDefaultEntry>(List<TDefaultEntry> defaultEntries)
            where TDefaultEntry : class, TEntry, IDefaultEntry;
    }
}