using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech.Data
{
    [Serializable]
    internal sealed class TextToSpeechRequest
    {
        [JsonProperty("input")] internal TextToSpeechInput Input { get; private set; }
        [JsonProperty("voice")] internal TextToSpeechVoice Voice { get; private set; }
        [JsonProperty("audioConfig")] internal TextToSpeechAudio Audio { get; private set; }

        internal TextToSpeechRequest(string text)
        {
            Input = new TextToSpeechInput(text);
            Voice = new TextToSpeechVoice("en-US", "en-US-Standard-A");
            Audio = new TextToSpeechAudio("MP3");
        }

        [JsonConstructor]
        internal TextToSpeechRequest(
            TextToSpeechInput input,
            TextToSpeechVoice voice,
            TextToSpeechAudio audio)
        {
            Input = input;
            Voice = voice;
            Audio = audio;
        }
    }
}