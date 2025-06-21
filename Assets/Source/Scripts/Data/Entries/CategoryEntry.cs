using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Data.Entries.Words;

namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct CategoryEntry
    {
        internal string CategoryName { get; set; }
        internal List<WordEntry> WordEntries { get; set; }
    }
}