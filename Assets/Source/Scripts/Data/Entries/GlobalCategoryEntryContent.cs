using System.Collections.Generic;
using MemoryPack;

// ReSharper disable MemberCanBePrivate.Global
namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct GlobalCategoryEntryContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EntryData<WordEntryContent>> Words { get; set; }
    }
}