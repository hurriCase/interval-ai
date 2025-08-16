using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Core.Sprites;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    [MemoryPackable]
    internal sealed partial class CategoryEntry : IDefaultEntry
    {
        public int Id { get; private set; }
        public CachedSprite Icon { get; private set; }
        public string LocalizationKey { get; private set; }
        public List<WordEntry> WordEntries { get; private set; } = new();
        public CategoryType CategoryType { get; private set; } = CategoryType.Default;
        public bool IsSelected { get; set; }
    }
}