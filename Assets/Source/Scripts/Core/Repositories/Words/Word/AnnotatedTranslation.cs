using CustomUtils.Runtime.Extensions;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal partial struct AnnotatedTranslation : ITranslation
    {
        public string Note { get; set; }
        public string Native { get; set; }
        public string Learning { get; set; }

        [MemoryPackIgnore]
        public readonly bool IsValid => Note.IsValid() && Native.IsValid() && Learning.IsValid();
    }
}