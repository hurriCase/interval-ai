using System;
using MemoryPack;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct WordEntry
    {
        [field: SerializeField] internal LearningState LearningState { get; set; }
        [field: SerializeField] internal Entries.Word NativeWord { get; set; }
        [field: SerializeField] internal Entries.Word LearningWord { get; set; }
        [field: SerializeField] internal string Transcription { get; set; }
    }
}