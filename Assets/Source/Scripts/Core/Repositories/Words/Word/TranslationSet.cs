using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal readonly partial struct TranslationSet : ITranslation
    {
        public string Learning { get; }
        public List<string> Natives { get; }

        internal TranslationSet(string learning, List<string> natives)
        {
            Learning = learning;
            Natives = natives;
        }

        [MemoryPackIgnore]
        public bool IsValid => Learning.IsValid() && Natives is { Count: > 0 } && Natives[0].IsValid();
    }
}