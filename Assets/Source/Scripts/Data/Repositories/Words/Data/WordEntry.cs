using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.Words.Base;
using UnityEditor;

namespace Source.Scripts.Data.Repositories.Words.Data
{
    [MemoryPackable]
    internal partial class WordEntry
    {
        public GUID GUID { get; } = GUID.Generate();
        public LearningState LearningState { get; set; }
        public Word NativeWord { get; set; }
        public Word LearningWord { get; set; }
        public Example Example { get; set; }
        public string Transcription { get; set; }
        public CachedSprite DescriptiveImage { get; set; }
        public int RepetitionCount { get; set; }
        public bool IsHidden { get; set; }
        public DateTime Cooldown { get; set; } = DateTime.MinValue;
    }
}