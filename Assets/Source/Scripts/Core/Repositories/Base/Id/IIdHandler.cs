using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;

namespace Source.Scripts.Core.Repositories.Base.Id
{
    internal interface IIdHandler<TEntry>
    {
        UniTask InitAsync(CancellationToken cancellationToken);
        Dictionary<int, TEntry> GenerateWithIds(List<TEntry> entries);

        public Dictionary<int, TEntry> GenerateWithDefaultIds<TDefaultEntry>(List<TDefaultEntry> defaultEntries)
            where TDefaultEntry : class, TEntry, IDefaultEntry;
    }
}