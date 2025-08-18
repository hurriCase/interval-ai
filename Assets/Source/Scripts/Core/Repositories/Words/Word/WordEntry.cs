using System;
using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Sprites;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal sealed partial class WordEntry
    {
        public List<int> CategoryIds { get; private set; } = new();
        public DateTime CreationData { get; private set; }
        public LearningState LearningState { get; private set; }
        public string NativeWord { get; set; }
        public string LearningWord { get; set; }
        public string NativeExample { get; set; }
        public string LearningExample { get; set; }
        public string Transcription { get; private set; }
        public CachedSprite DescriptiveImage { get; set; }
        public int ReviewCount { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime Cooldown { get; private set; } = DateTime.MinValue;
    }
}