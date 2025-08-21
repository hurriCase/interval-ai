using System.Collections.Generic;

namespace Source.Scripts.Core.Repositories.Base.DefaultConfig
{
    internal interface IDefaultDataDatabase<TEntry>
    {
        List<TEntry> Defaults { get; }
    }
}