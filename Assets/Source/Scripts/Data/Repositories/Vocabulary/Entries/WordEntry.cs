using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial class WordEntry
    {
        [field: SerializeField] internal LearningState LearningState { get; set; }
        [field: SerializeField] internal Word NativeWord { get; set; }
        [field: SerializeField] internal Word LearningWord { get; set; }
        [field: SerializeField] internal Example Example { get; set; }
        [field: SerializeField] internal string Transcription { get; set; }
        [field: SerializeField] internal Sprite DescriptiveImage { get; set; }
        [field: SerializeField] internal int RepetitionCount { get; set; }
        [field: SerializeField] internal bool IsHidden { get; set; }

        internal DateTime Cooldown { get; set; } = DateTime.MinValue;

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
    }
}