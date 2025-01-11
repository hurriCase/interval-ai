using System;
using Client.Scripts.DB.Entities.Base.Validation;
using Newtonsoft.Json;

namespace Client.Scripts.DB.Entities.ProgressEntity
{
    [Serializable]
    internal sealed class ProgressEntryContent
    {
        //TODO:<dmitriy.sukharev> think about better approach to pass restrictions
        [JsonProperty("wordId")]
        [StringLength(1, 100)]
        [Required]
        public string WordId { get; set; }

        [JsonProperty("repetitionStage")]
        [NumericRange(1, 5)]
        [Required]
        public int RepetitionStage { get; set; }

        [JsonProperty("totalReviews")] public int TotalReviews { get; set; }
        [JsonProperty("correctReviews")] public int CorrectReviews { get; set; }
        [JsonProperty("lastReviewDate")] public DateTime LastReviewDate { get; set; }
        [JsonProperty("nextReviewDate")] public DateTime NextReviewDate { get; set; }
    }
}