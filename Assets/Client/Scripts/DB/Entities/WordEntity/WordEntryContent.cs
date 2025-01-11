using System;
using System.Collections.Generic;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.WordEntity
{
    [Serializable]
    internal sealed class WordEntryContent
    {
        [JsonProperty("categoryId")]
        [Required]
        public string CategoryId { get; set; }

        [JsonProperty("nativeWord")]
        [StringLength(1, 100)]
        [Required]
        public string NativeWord { get; set; }

        [JsonProperty("learningWord")]
        [StringLength(1, 100)]
        [Required]
        public string LearningWord { get; set; }

        [JsonProperty("transcription")] public string Transcription { get; set; }
        [JsonProperty("examples")] public List<Example> Examples { get; set; } = new();
        [JsonProperty("isDefault")] public bool IsDefault { get; set; }

        [Serializable]
        internal sealed class Example
        {
            [JsonProperty("nativeSentence")] public string NativeSentence { get; set; }
            [JsonProperty("learningSentence")] public string LearningSentence { get; set; }
        }
    }
}