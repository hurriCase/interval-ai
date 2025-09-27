using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LanguageDetector
{
    internal sealed class LanguageDetector : ILanguageDetector
    {
        private readonly LanguageDetection.LanguageDetector _languageDetector;

        internal LanguageDetector()
        {
            _languageDetector = new LanguageDetection.LanguageDetector();
            _languageDetector.AddAllLanguages();
        }

        public SystemLanguage DetectLanguage(string text) => _languageDetector.Detect(text).ISOToSystemLanguage();
    }
}