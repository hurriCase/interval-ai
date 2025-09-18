using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.Core.Localization.LanguageDetector
{
    internal sealed class LanguageLanguageDetector : ILanguageDetector
    {
        private readonly LanguageDetection.LanguageDetector _languageDetector;

        internal LanguageLanguageDetector()
        {
            _languageDetector = new LanguageDetection.LanguageDetector();
            _languageDetector.AddAllLanguages();
        }

        public SystemLanguage DetectLanguage(string text) => _languageDetector.Detect(text).ISOToSystemLanguage();
    }
}