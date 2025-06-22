using System;
using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Data.Repositories.Entries.Words;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct CategoryEntry
    {
        [field: SerializeField] internal Sprite Icon { get; set; }
        [field: SerializeField] internal string LocalizationKey { get; set; }
        [field: SerializeField] internal List<WordEntry> WordEntries { get; set; }
    }
}