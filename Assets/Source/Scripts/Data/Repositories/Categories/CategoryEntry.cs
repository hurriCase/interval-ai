using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Data.Repositories.Words.Data;

namespace Source.Scripts.Data.Repositories.Categories
{
    [MemoryPackable]
    internal partial struct CategoryEntry
    {
        public CachedSprite Icon { get; set; }
        public string LocalizationKey { get; set; }
        public List<WordEntry> WordEntries { get; set; }
        public CategoryType CategoryType { get; set; }
    }
}