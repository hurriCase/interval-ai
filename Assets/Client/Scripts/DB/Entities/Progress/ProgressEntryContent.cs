using System;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.Progress
{
    [Serializable]
    internal sealed class ProgressEntryContent
    {
        [JsonProperty("wordId")] [Validation] public string WordId { get; set; }

        [JsonProperty("repetitionStage")]
        [Validation]
        public int RepetitionStage { get; set; }

        [JsonProperty("totalReviews")]
        [Validation]
        public int TotalReviews { get; set; }

        [JsonProperty("correctReviews")]
        [Validation]
        public int CorrectReviews { get; set; }

        [JsonProperty("lastReviewDate")]
        [Validation]
        public DateTime LastReviewDate { get; set; }

        [JsonProperty("nextReviewDate")]
        [Validation]
        public DateTime NextReviewDate { get; set; }
    }
}