using System;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Entries.Words
{
    [MemoryPackable]
    [Serializable]
    internal partial struct Word
    {
        [field: SerializeField] internal string Name { get; set; }
        [field: SerializeField] internal Language Language { get; set; }
    }
}