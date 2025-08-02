using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words;

namespace Source.Scripts.Core.Repositories.Categories
{
    [MemoryPackable]
    internal partial struct CategoryEntry
    {
        public CachedSprite Icon { get; set; }
        public string LocalizationKey { get; set; }
        public CategoryType CategoryType { get; set; }
    }
}