using System.Collections.Generic;
using CustomUtils.Runtime.AddressableSystem;
using MemoryPack;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    [MemoryPackable]
    internal sealed partial class CategoryEntry : IDefaultEntry, IEntry
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public CachedSprite Icon { get; private set; }
        public WordOrderType WordOrderType { get; private set; } = WordOrderType.Default;
        public List<WordEntry> WordEntries { get; private set; } = new();
        public CategoryType CategoryType { get; private set; }
        public bool IsSelected { get; set; }
        public string GetName() => Name;
    }
}