using System;
using System.Threading.Tasks;
using Client.Scripts.DB.Base;
using UnityEngine;

namespace Client.Scripts.DB.Entities
{
    internal sealed class ProgressEntity : DBEntityBase<ProgressEntityData>
    {
        internal async Task UpdateWordProgress(string wordId, bool wasCorrect)
        {
            var repetitionStage = wasCorrect ? 1 : 0;
            var progressData = new ProgressEntityData
            {
                WordId = wordId,
                TotalReviews = 1,
                RepetitionStage = repetitionStage,
                LastReviewDate = DateTime.UtcNow,
                NextReviewDate = CalculateNextReviewDate(repetitionStage)
            };

            await CreateEntityAsync(progressData);
        }

        //TODO:<dmitriy.sukharev> Make that user can change intervals
        private static DateTime CalculateNextReviewDate(int repetitionStage)
        {
            var hoursToAdd = repetitionStage switch
            {
                0 => 1,   // 1 hour
                1 => 3,   // 3 hours
                2 => 8,   // 8 hours
                3 => 24,  // 1 day
                4 => 72,  // 3 days
                5 => 168, // 1 week
                _ => 1
            };

            return DateTime.UtcNow.AddHours(hoursToAdd);
        }

        protected override string GetPath() => "user_progress";
    }

    [Serializable]
    internal struct ProgressEntityData
    {
        [field: SerializeField] public string WordId { get; set; }
        [field: SerializeField] public int RepetitionStage { get; set; }
        [field: SerializeField] public int TotalReviews { get; set; }
        [field: SerializeField] public int CorrectReviews { get; set; }
        [field: SerializeField] public DateTime LastReviewDate { get; set; }
        [field: SerializeField] public DateTime NextReviewDate { get; set; }
    }
}