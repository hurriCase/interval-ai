using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;
using Example = Source.Scripts.Data.Repositories.Categories.Entries.Example;

namespace Source.Scripts.Data.Repositories.Words
{
    [MemoryPackable]
    [Serializable]
    internal partial class WordEntry : IEquatable<WordEntry>
    {
        [field: SerializeField] public LearningState LearningState { get; set; }
        [field: SerializeField] public Word NativeWord { get; set; }
        [field: SerializeField] public Word LearningWord { get; set; }
        [field: SerializeField] public Example Example { get; set; }
        [field: SerializeField] public string Transcription { get; set; }
        public CachedSprite DescriptiveImage { get; set; }
        [field: SerializeField] public int RepetitionCount { get; set; }
        [field: SerializeField] public bool IsHidden { get; set; }

        public DateTime Cooldown { get; set; } = DateTime.MinValue;

        public bool IsValid => string.IsNullOrEmpty(NativeWord.Name) is false ||
                               string.IsNullOrEmpty(LearningWord.Name) is false;

        public bool Equals(WordEntry other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return NativeWord.Name == other.NativeWord.Name &&
                   LearningWord.Name == other.LearningWord.Name &&
                   NativeWord.Language == other.NativeWord.Language &&
                   LearningWord.Language == other.LearningWord.Language;
        }

        public override bool Equals(object obj) => Equals(obj as WordEntry);

        public override int GetHashCode() =>
            HashCode.Combine(
                NativeWord.Name?.GetHashCode() ?? 0,
                LearningWord.Name?.GetHashCode() ?? 0,
                NativeWord.Language,
                LearningWord.Language
            );

        public static bool operator ==(WordEntry left, WordEntry right) => Equals(left, right);

        public static bool operator !=(WordEntry left, WordEntry right) => !Equals(left, right);
    }
}