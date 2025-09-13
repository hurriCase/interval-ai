using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechResponse
    {
        [JsonProperty("audioContent")] internal string AudioContent { get; private set; }

        [JsonConstructor]
        internal TextToSpeechResponse(string audioContent)
        {
            AudioContent = audioContent;
        }
    }
}