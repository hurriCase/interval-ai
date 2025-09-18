using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Localization.Translator.Data
{
    [Serializable]
    internal sealed class TranslationResult
    {
        [JsonProperty("text")] internal string Text { get; private set; }
        [JsonProperty("to")] internal string To { get; private set; }

        [JsonConstructor]
        internal TranslationResult(string text, string to)
        {
            Text = text;
            To = to;
        }
    }
}