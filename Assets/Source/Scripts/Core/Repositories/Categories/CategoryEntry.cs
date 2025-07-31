using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Core.DI.Repositories.Words;
using Source.Scripts.Core.Others;

namespace Source.Scripts.Core.DI.Repositories.Categories
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