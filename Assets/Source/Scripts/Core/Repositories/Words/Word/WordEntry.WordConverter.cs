using CustomUtils.Runtime.CSV.Base;
using CustomUtils.Runtime.CSV.CSVEntry;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Localization.Translator.Translations;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal sealed partial class WordEntry
    {
        [Preserve]
        internal sealed class WordConverter : CsvConverterBase<WordEntry>
        {
            private const string WordName = "Word";

            private const string TranscriptionName = "Transcription";
            private const string CategoryIdsName = "CategoryIds";

            private const string ExamplesName = "Examples";
            private const string TranslationVariantsName = "TranslationVariants";
            private const string SynonymsName = "Synonyms";
            private const string GrammarName = "Grammar";

            private readonly ITranslationParser _translationParser;

            [Preserve]
            internal WordConverter(ITranslationParser translationParser)
            {
                _translationParser = translationParser;
            }

            protected override WordEntry ConvertRow(CsvRow row) =>
                new()
                {
                    Word = _translationParser.GetTranslationSet(row, WordName),
                    Transcription = row.GetValue(TranscriptionName),
                    CategoryIds = row.GetValue(CategoryIdsName).ToIntList(),
                    Examples = _translationParser.GetTranslatedList(row, ExamplesName),
                    TranslationVariants = _translationParser.GetTranslatedList(row, TranslationVariantsName),
                    Synonyms = _translationParser.GetTranslatedSetList(row, SynonymsName),
                    Grammar = _translationParser.GetAnnotatedTranslationList(row, GrammarName),
                };
        }
    }
}