using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechVoice
    {
        [JsonProperty("languageCode")] internal string LanguageCode { get; private set; }
        [JsonProperty("name")] internal string Name { get; private set; }

        internal TextToSpeechVoice(string languageCode, string name)
        {
            LanguageCode = languageCode;
            Name = name;
        }
    }
}