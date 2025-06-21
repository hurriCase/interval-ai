using System;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Entries.Words
{
    [MemoryPackable]
    [Serializable]
    internal partial struct WordEntry
    {
        [field: SerializeField] internal LearningState LearningState { get; set; }
        [field: SerializeField] internal Word NativeWord { get; set; }
        [field: SerializeField] internal Word LearningWord { get; set; }
        [field: SerializeField] internal string Transcription { get; set; }
    }
}