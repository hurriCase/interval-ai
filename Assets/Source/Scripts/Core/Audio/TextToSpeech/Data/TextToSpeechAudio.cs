using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechAudio
    {
        [JsonProperty("audioEncoding")] internal string AudioEncoding { get; private set; }

        internal TextToSpeechAudio(string audioEncoding)
        {
            AudioEncoding = audioEncoding;
        }
    }
}