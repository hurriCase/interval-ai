using UnityEngine;

namespace Source.Scripts.Core.Localization.LanguageDetector
{
    internal interface ILanguageDetector
    {
        SystemLanguage DetectLanguage(string text);
    }
}