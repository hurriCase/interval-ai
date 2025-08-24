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
        public CachedSprite DescriptiveImage { get; private set; }
        public string Transcription { get; private set; }
        public TranslationSet Word { get; set; }
        public List<int> CategoryIds { get; private set; } = new();
        public List<Translation> Examples { get; private set; }
        public List<Translation> TranslationVariants { get; private set; }
        public List<TranslationSet> Synonyms { get; private set; }
        public List<AnnotatedTranslation> Grammar { get; private set; }
        public LearningState LearningState { get; private set; }
        public int ReviewCount { get; private set; }
        public bool IsHidden { get; private set; }
        public DateTime CreationData { get; private set; }
        public DateTime Cooldown { get; private set; } = DateTime.MinValue;
    }
}