using CustomUtils.Runtime.Extensions;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal readonly partial struct Translation : ITranslation
    {
        public string Native { get; }
        public string Learning { get; }

        public Translation(string native, string learning)
        {
            Native = native;
            Learning = learning;
        }

        [MemoryPackIgnore]
        public bool IsValid => Native.IsValid() && Learning.IsValid();
    }
}