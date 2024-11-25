using System;
using System.Threading.Tasks;

namespace Client.Scripts.Database.Vocabulary
{
    internal sealed class ProgressEntityData
    {
        public string WordId { get; set; }
        public int RepetitionStage { get; set; }
        public int TotalReviews { get; set; }
        public int CorrectReviews { get; set; }
        public DateTime LastReviewDate { get; set; }
        public DateTime NextReviewDate { get; set; }
    }

    internal sealed class ProgressEntity : DataBaseEntity<ProgressEntityData>, IInitializable
    {
        protected override string GetPath(string userId) => $"user_progress/{userId}";

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
    }
}