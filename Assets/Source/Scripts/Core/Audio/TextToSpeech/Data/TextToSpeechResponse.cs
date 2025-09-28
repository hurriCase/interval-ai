using System;
using Newtonsoft.Json;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechResponse : IValidatable
    {
        [JsonProperty("audioContent")] internal string AudioContent { get; private set; }

        [JsonConstructor]
        internal TextToSpeechResponse(string audioContent)
        {
            AudioContent = audioContent;
        }

        public bool IsValid() => string.IsNullOrEmpty(AudioContent) is false;
    }
}