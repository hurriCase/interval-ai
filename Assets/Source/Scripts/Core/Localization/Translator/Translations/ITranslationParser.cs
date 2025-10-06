using System.Collections.Generic;
using CustomUtils.Runtime.CSV.CSVEntry;

namespace Source.Scripts.Core.Localization.Translator.Translations
{
    internal interface ITranslationParser
    {
        Translation GetTranslation(CsvRow row, string translationName);
        TranslationSet GetTranslationSet(CsvRow row, string translationName);
        List<Translation> GetTranslatedList(CsvRow row, string translationName);
        List<TranslationSet> GetTranslatedSetList(CsvRow row, string translationName);
        List<AnnotatedTranslation> GetAnnotatedTranslationList(CsvRow row, string translationName);
    }
}