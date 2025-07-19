using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.User;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary.Entries
{
    [MemoryPackable]
    [Serializable]
    internal partial struct Example
    {
        [field: SerializeField] public string NativeExample { get; set; }
        [field: SerializeField] public string LearningExample { get; set; }

        internal string ShownExample =>
            UserRepository.Instance.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? LearningExample
                : NativeExample;

        internal string HiddeExample =>
            UserRepository.Instance.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? NativeExample
                : LearningExample;
    }
}