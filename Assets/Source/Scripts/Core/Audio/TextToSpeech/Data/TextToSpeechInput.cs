using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechInput
    {
        [JsonProperty("text")] internal string Text { get; private set; }

        internal TextToSpeechInput(string text)
        {
            Text = text;
        }
    }
}