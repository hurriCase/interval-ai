using System;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Categories.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct Example
    {
        [field: SerializeField] public string NativeExample { get; set; }
        [field: SerializeField] public string LearningExample { get; set; }
    }
}