using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    [MemoryPackable]
    internal readonly partial struct AnnotatedTranslation : ITranslation
    {
        public string Note { get; }
        public Translation Translation { get; }

        internal AnnotatedTranslation(string note, Translation translation)
        {
            Note = note;
            Translation = translation;
        }

        [MemoryPackIgnore]
        public bool IsValid => string.IsNullOrEmpty(Note) is false && Translation.IsValid;
    }
}