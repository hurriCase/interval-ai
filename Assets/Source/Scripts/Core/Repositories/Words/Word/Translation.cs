using CustomUtils.Runtime.Extensions;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal partial struct Translation : ITranslation
    {
        public string Native { get; set; }
        public string Learning { get; set; }

        [MemoryPackIgnore]
        public readonly bool IsValid => Native.IsValid() && Learning.IsValid();
    }
}