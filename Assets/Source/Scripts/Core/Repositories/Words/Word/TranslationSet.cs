using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal partial struct TranslationSet : ITranslation
    {
        public string Learning { get; set; }
        public List<string> Natives { get; set; }

        [MemoryPackIgnore]
        public readonly bool IsValid => Learning.IsValid() && Natives is { Count: > 0 } && Natives[0].IsValid();
    }
}