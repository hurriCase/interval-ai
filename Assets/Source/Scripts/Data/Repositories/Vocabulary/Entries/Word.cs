using System;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct Word
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Language Language { get; set; }
    }
}