using System;
using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Words;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Categories.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct CategoryEntry
    {
        public CachedSprite Icon { get; set; }
        [field: SerializeField] public string LocalizationKey { get; set; }
        [field: SerializeField] public List<WordEntry> WordEntries { get; set; }
    }
}