using CustomUtils.Runtime.CSV.Base;
using CustomUtils.Runtime.CSV.CSVEntry;
using Source.Scripts.Core.Localization.Translator.Translations;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Exercises.Exercise
{
    internal sealed partial class ExerciseEntry
    {
        [Preserve]
        internal sealed class ExerciseConverter : CsvConverterBase<ExerciseEntry>
        {
            private const string ExerciseContentName = "Content";

            private readonly ITranslationParser _translationParser;

            [Preserve]
            internal ExerciseConverter(ITranslationParser translationParser)
            {
                _translationParser = translationParser;
            }

            protected override ExerciseEntry ConvertRow(CsvRow row) =>
                new()
                {
                    Content = _translationParser.GetTranslation(row, ExerciseContentName)
                };
        }
    }
}