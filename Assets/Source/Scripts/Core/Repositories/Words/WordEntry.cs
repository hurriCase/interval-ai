using System;
using MemoryPack;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Others;
using UnityEditor;

namespace Source.Scripts.Core.DI.Repositories.Words
{
    [MemoryPackable]
    internal sealed partial class WordEntry
    {
        public GUID GUID { get; } = GUID.Generate();
        public LearningState LearningState { get; set; }
        public string NativeWord { get; set; }
        public string LearningWord { get; set; }
        public string NativeExample { get; set; }
        public string LearningExample { get; set; }
        public string Transcription { get; set; }
        public CachedSprite DescriptiveImage { get; set; }
        public int RepetitionCount { get; set; }
        public bool IsHidden { get; set; }
        public DateTime Cooldown { get; set; } = DateTime.MinValue;
    }
}