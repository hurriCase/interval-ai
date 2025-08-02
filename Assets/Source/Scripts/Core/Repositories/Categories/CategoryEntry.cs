using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Core.Importer;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words;

namespace Source.Scripts.Core.Repositories.Categories
{
    [MemoryPackable]
    internal partial class CategoryEntry : IDefaultEntry
    {
        public int DefaultId { get; private set; }
        public CachedSprite Icon { get; private set; }
        public string LocalizationKey { get; private set; }
        public List<WordEntry> WordEntries { get; private set; } = new();
        public CategoryType CategoryType { get; private set; } = CategoryType.Default;
    }
}