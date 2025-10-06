using MemoryPack;
using Source.Scripts.Core.Localization.Translator.Translations;

namespace Source.Scripts.Core.Repositories.Exercises.Exercise
{
    [MemoryPackable]
    internal sealed partial class ExerciseEntry
    {
        public int Id { get; private set; }
        public Translation Content { get; private set; }
    }
}