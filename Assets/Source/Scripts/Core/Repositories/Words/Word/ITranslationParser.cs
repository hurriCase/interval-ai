using System.Collections.Generic;
using CustomUtils.Runtime.CSV.CSVEntry;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal interface ITranslationParser
    {
        TranslationSet GetTranslationSet(CsvRow row, string translationName);
        List<Translation> GetTranslatedList(CsvRow row, string translationName);
        List<TranslationSet> GetTranslatedSetList(CsvRow row, string translationName);
        List<AnnotatedTranslation> GetAnnotatedTranslationList(CsvRow row, string translationName);
    }
}