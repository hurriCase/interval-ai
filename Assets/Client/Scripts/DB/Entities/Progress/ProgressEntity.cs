using System;
using System.Threading.Tasks;
using Client.Scripts.DB.Entities.Base;

namespace Client.Scripts.DB.Entities.Progress
{
    [SingleInstanceEntry]
    internal sealed class ProgressEntity : EntityBase<ProgressEntryContent>
    {
        protected override string EntityPath => "category_entity";

        internal async Task UpdateWordProgress(string wordId, bool wasCorrect)
        {
            var repetitionStage = wasCorrect ? 1 : 0;
            var progressData = new ProgressEntryContent
            {
                WordId = wordId,
                TotalReviews = 1,
                RepetitionStage = repetitionStage,
                LastReviewDate = DateTime.UtcNow,
                NextReviewDate = CalculateNextReviewDate(repetitionStage)
            };

            await CreateEntryAsync(progressData);
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