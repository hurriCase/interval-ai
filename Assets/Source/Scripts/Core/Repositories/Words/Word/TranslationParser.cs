using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CSV.CSVEntry;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal sealed class TranslationParser : ITranslationParser
    {
        private const string LearningName = "Learning";
        private const string NativeName = "Native";

        private const string NoteName = "Note";

        private const string DelimiterPropertyPattern = "{0}.{1}";
        private const string NumericPropertyPattern = @"{0}(\d+)\.{1}";

        public TranslationSet GetTranslationSet(CsvRow row, string translationName)
        {
            var learningWordPattern = ZString.Format(DelimiterPropertyPattern, translationName, LearningName);
            var nativeWordPattern = ZString.Format(DelimiterPropertyPattern, translationName, NativeName);

            return new TranslationSet(
                row.GetValueByPattern(learningWordPattern),
                row.GetValueByPattern(nativeWordPattern).ToStringList());
        }

        public List<Translation> GetTranslatedList(CsvRow row, string translationName)
        {
            var learningName = ZString.Format(DelimiterPropertyPattern, translationName, NativeName);
            var nativeName = ZString.Format(DelimiterPropertyPattern, translationName, LearningName);

            var learningValues = row.GetValue(learningName).ToStringList();
            var nativeValues = row.GetValue(nativeName).ToStringList();

            return ProcessTranslations(nativeValues, learningValues,
                (native, learning) => new Translation(native, learning));
        }

        public List<TranslationSet> GetTranslatedSetList(CsvRow row, string translationName)
        {
            var learningPattern = ZString.Format(NumericPropertyPattern, translationName, NativeName);
            var nativePattern = ZString.Format(NumericPropertyPattern, translationName, LearningName);

            var learningValues = row.GetValuesByPattern(learningPattern);
            var nativeValues = row.GetValuesByPattern(nativePattern);

            return ProcessTranslations(learningValues, nativeValues,
                (learning, natives) => new TranslationSet(learning, natives.ToStringList()));
        }

        private static List<T> ProcessTranslations<T>(
            IReadOnlyList<string> firstValues,
            IReadOnlyList<string> secondValues,
            Func<string, string, T> createItem)
        {
            var results = new List<T>();
            var minCount = Mathf.Min(firstValues.Count, secondValues.Count);

            for (var i = 0; i < minCount; i++)
            {
                if (string.IsNullOrEmpty(firstValues[i]) is false && string.IsNullOrEmpty(secondValues[i]) is false)
                    results.Add(createItem(firstValues[i], secondValues[i]));
            }

            return results;
        }

        public List<AnnotatedTranslation> GetAnnotatedTranslationList(CsvRow row, string translationName)
        {
            var noteName = ZString.Format(DelimiterPropertyPattern, translationName, NoteName);
            var learningName = ZString.Format(DelimiterPropertyPattern, translationName, NativeName);
            var nativeName = ZString.Format(DelimiterPropertyPattern, translationName, LearningName);

            var noteValues = row.GetValue(noteName).ToStringList();
            var nativeValues = row.GetValue(learningName).ToStringList();
            var learningValues = row.GetValue(nativeName).ToStringList();

            var results = new List<AnnotatedTranslation>();
            var minCount = Mathf.Min(Mathf.Min(noteValues.Count, nativeValues.Count), learningValues.Count);

            for (var i = 0; i < minCount; i++)
            {
                var note = noteValues[i];
                var native = nativeValues[i];
                var learning = learningValues[i];

                if (string.IsNullOrEmpty(note) || string.IsNullOrEmpty(native) || string.IsNullOrEmpty(learning))
                    continue;

                var translation = new Translation(native, learning);
                var annotatedTranslation = new AnnotatedTranslation(note, translation);

                if (annotatedTranslation.IsValid is false)
                    continue;

                results.Add(annotatedTranslation);
            }

            return results;
        }
    }
}