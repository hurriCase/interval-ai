using System.Collections.Generic;

namespace Source.Scripts.Core.Repositories
{
    internal interface IIdHandler<TEntry>
    {
        Dictionary<int, TEntry> GenerateWithIds(List<TEntry> entries);

        Dictionary<int, TEntry> GenerateWithDefaultIds<TDefaultEntry>(List<TDefaultEntry> defaultEntries)
            where TDefaultEntry : class, IDefaultEntry<TEntry>, new();
    }
}