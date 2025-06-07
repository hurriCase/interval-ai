using System.Collections.Generic;
using MemoryPack;

namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct UserCategoryEntryContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EntryData<WordEntryContent>> Words { get; set; }
    }
}