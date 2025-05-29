using CustomUtils.Runtime.Extensions;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.Core.AI
{
    internal sealed class GenerationConfig
    {
        [Tooltip("List of sequences where the model should stop generating further tokens")]
        [JsonProperty("stopSequences")]
        internal string[] StopSequences { get; set; }

        [JsonProperty("responseMimeType")]
        internal string ResponseMimeTypeString => ResponseMimeType.GetJsonPropertyName();

        [Tooltip("Specifies the format of the response (e.g. 'text/plain', 'application/json')")]
        [JsonIgnore]
        internal ResponseMimeType ResponseMimeType { get; set; }

        // [Tooltip("Defines the structure that response should follow")]
        // [JsonProperty("responseSchema")]
        // internal object ResponseSchema { get; set; }

        [Tooltip("Number of alternative responses to generate")]
        [JsonProperty("candidateCount")]
        internal int CandidateCount { get; set; } = 1;

        [Tooltip("Maximum number of tokens to generate in the response")]
        [JsonProperty("maxOutputTokens")]
        internal int MaxOutputTokens { get; set; } = 1000;

        [Tooltip("Controls randomness (0.0 = focused/deterministic, 1.0 = more creative)")]
        [JsonProperty("temperature")]
        internal float Temperature { get; set; } = 1.0f;

        [Tooltip("Probability threshold for token selection (lower = more focused on likely tokens)")]
        [JsonProperty("topP")]
        internal float TopP { get; set; } = 0.95f;

        [Tooltip("Limits the number of tokens considered for generation (lower = more focused responses)")]
        [JsonProperty("topK")]
        internal int TopK { get; set; } = 40;

        [Tooltip("Penalizes tokens based on their presence in the text")]
        [JsonProperty("presencePenalty")]
        internal float PresencePenalty { get; set; }

        [Tooltip("Penalizes tokens based on their frequency in the text")]
        [JsonProperty("frequencyPenalty")]
        internal float FrequencyPenalty { get; set; }

        [Tooltip("Include token probabilities in the response")]
        [JsonProperty("responseLogprobs")]
        internal bool ResponseLogprobs { get; set; }

        [Tooltip("Number of most probable tokens to return per position")]
        [JsonProperty("logprobs", NullValueHandling = NullValueHandling.Ignore)]
        internal int? Logprobs
        {
            get => ResponseLogprobs ? _logprobs : null;
            set => _logprobs = value ?? 3;
        }

        private int _logprobs = 3;
    }

    internal enum ResponseMimeType
    {
        [JsonProperty("text/plain")] TextPlain,

        [JsonProperty("application/json")] Json,

        [JsonProperty("application/xml")] Xml,

        [JsonProperty("text/html")] Html,

        [JsonProperty("text/markdown")] Markdown
    }
}