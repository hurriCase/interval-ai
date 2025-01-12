using System;
using System.Collections.Generic;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.Word
{
    [Serializable]
    internal sealed class WordEntryContent
    {
        [JsonProperty("categoryId")]
        [Validation]
        public string CategoryId { get; set; }

        [JsonProperty("nativeWord")]
        [Validation]
        public string NativeWord { get; set; }

        [JsonProperty("learningWord")]
        [Validation]
        public string LearningWord { get; set; }

        [JsonProperty("transcription")]
        [Validation]
        public string Transcription { get; set; }

        [JsonProperty("examples")]
        [Validation]
        public List<Example> Examples { get; set; } = new();

        [JsonProperty("isDefault")]
        [Validation]
        public bool IsDefault { get; set; }

        [Serializable]
        internal sealed class Example
        {
            [JsonProperty("nativeSentence")]
            [Validation]
            public string NativeSentence { get; set; }

            [JsonProperty("learningSentence")]
            [Validation]
            public string LearningSentence { get; set; }
        }
    }
}