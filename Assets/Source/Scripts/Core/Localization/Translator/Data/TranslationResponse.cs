using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Localization.Translator.Data
{
    [Serializable]
    internal sealed class TranslationResponse
    {
        [JsonProperty("translations")] internal TranslationResult[] Translations { get; private set; }

        [JsonConstructor]
        internal TranslationResponse(TranslationResult[] translations)
        {
            Translations = translations;
        }
    }
}