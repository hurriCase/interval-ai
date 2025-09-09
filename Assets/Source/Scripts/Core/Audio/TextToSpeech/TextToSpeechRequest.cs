using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    [Serializable]
    internal sealed class TextToSpeechRequest
    {
        [JsonProperty("input")] internal TextToSpeechInput Input { get; private set; }
        [JsonProperty("voice")] internal TextToSpeechVoice Voice { get; private set; }
        [JsonProperty("audioConfig")] internal TextToSpeechAudioConfig AudioConfig { get; private set; }

        internal TextToSpeechRequest(string text)
        {
            Input = new TextToSpeechInput(text);
            Voice = new TextToSpeechVoice("en-US", "en-US-Standard-A");
            AudioConfig = new TextToSpeechAudioConfig("MP3");
        }

        [JsonConstructor]
        internal TextToSpeechRequest(
            TextToSpeechInput input,
            TextToSpeechVoice voice,
            TextToSpeechAudioConfig audioConfig)
        {
            Input = input;
            Voice = voice;
            AudioConfig = audioConfig;
        }
    }

    [Serializable]
    internal sealed class TextToSpeechInput
    {
        [JsonProperty("text")] internal string Text { get; private set; }

        internal TextToSpeechInput(string text)
        {
            Text = text;
        }
    }

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

    [Serializable]
    internal sealed class TextToSpeechAudioConfig
    {
        [JsonProperty("audioEncoding")] internal string AudioEncoding { get; private set; }

        internal TextToSpeechAudioConfig(string audioEncoding)
        {
            AudioEncoding = audioEncoding;
        }
    }

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