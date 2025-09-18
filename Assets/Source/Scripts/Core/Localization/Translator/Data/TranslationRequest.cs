using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Localization.Translator.Data
{
    [Serializable]
    internal sealed class TranslationRequest
    {
        [JsonProperty("Text")] internal string Text { get; private set; }

        internal TranslationRequest(string text)
        {
            Text = text;
        }
    }
}