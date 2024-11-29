using System;
using System.Threading.Tasks;
using Client.Scripts.Database.Base;

namespace Client.Scripts.Database.Entities
{
    internal sealed class ProgressEntity : DataBaseEntity<ProgressEntityData>
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

            await CreateEntity(progressData);
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

        protected override string GetPath(string userId) => $"user_progress/{userId}";
    }

    internal sealed class ProgressEntityData
    {
        internal string WordId { get; set; }
        internal int RepetitionStage { get; set; }
        internal int TotalReviews { get; set; }
        internal int CorrectReviews { get; set; }
        internal DateTime LastReviewDate { get; set; }
        internal DateTime NextReviewDate { get; set; }
    }
}