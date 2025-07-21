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
        //TODO:<Dmitriy.Sukharev> Store sprite path and load from addressables
        [MemoryPackIgnore]
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public string LocalizationKey { get; set; }
        [field: SerializeField] public List<WordEntry> WordEntries { get; set; }
    }
}