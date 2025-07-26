using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Words.Data
{
    [MemoryPackable]
    [Serializable]
    internal partial struct Word
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Language Language { get; set; }
    }
}