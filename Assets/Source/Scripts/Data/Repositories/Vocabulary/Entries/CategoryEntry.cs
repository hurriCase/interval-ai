using System;
using System.Collections.Generic;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
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