using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
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

        //TODO:<Dmitriy.Sukharev> Store sprite path and load from addressables
        [MemoryPackIgnore]
        [field: SerializeField] public Sprite DescriptiveImage { get; set; }
        [field: SerializeField] public int RepetitionCount { get; set; }
        [field: SerializeField] public bool IsHidden { get; set; }

        public DateTime Cooldown { get; set; } = DateTime.MinValue;

        public bool IsValid => string.IsNullOrEmpty(NativeWord.Name) is false ||
                               string.IsNullOrEmpty(LearningWord.Name) is false;

        internal string ShownWord =>
            UserRepository.Instance.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? LearningWord.Name
                : NativeWord.Name;

        internal string HiddenWord =>
            UserRepository.Instance.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? NativeWord.Name
                : LearningWord.Name;

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