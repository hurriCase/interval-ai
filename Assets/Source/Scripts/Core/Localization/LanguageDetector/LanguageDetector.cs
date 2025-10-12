using CustomUtils.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Localization.LanguageDetector
{
    [Preserve]
    internal sealed class LanguageDetector : ILanguageDetector
    {
        private readonly LanguageDetection.LanguageDetector _languageDetector;

        [Preserve]
        internal LanguageDetector()
        {
            _languageDetector = new LanguageDetection.LanguageDetector();
            _languageDetector.AddAllLanguages();
        }

        public SystemLanguage DetectLanguage(string text) => _languageDetector.Detect(text).ISOToSystemLanguage();
    }
}