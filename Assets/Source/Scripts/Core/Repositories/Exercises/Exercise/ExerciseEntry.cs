using MemoryPack;
using Source.Scripts.Core.Localization.Translator.Translations;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.Repositories.Exercises.Exercise
{
    [MemoryPackable]
    internal sealed partial class ExerciseEntry : IEntry
    {
        public int Id { get; private set; }
        public Translation Content { get; private set; }
        public string GetName() => Content.Learning;
    }
}