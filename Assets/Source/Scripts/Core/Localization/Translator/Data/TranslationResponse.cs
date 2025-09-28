using System;
using Newtonsoft.Json;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.Localization.Translator.Data
{
    [Serializable]
    internal sealed class TranslationResponse : IValidatable
    {
        [JsonProperty("translations")] internal TranslationResult[] Translations { get; private set; }

        [JsonConstructor]
        internal TranslationResponse(TranslationResult[] translations)
        {
            Translations = translations;
        }

        public bool IsValid() => Translations is { Length: > 0 };
        internal string GetText() => Translations[0].Text;
    }
}