using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Database.Base;

namespace Client.Scripts.Database.Entities
{
    internal sealed class WordEntity : DataBaseEntity<WordEntityData>
    {
        internal List<EntityData<WordEntityData>> GetWordsForReview(
            Dictionary<string, EntityData<ProgressEntityData>> userProgress,
            Dictionary<string, EntityData<CategoryEntityData>> categories)
        {
            var now = DateTime.UtcNow;
            var wordsForReview = new List<EntityData<WordEntityData>>();

            foreach (var progress in userProgress.Values)
            {
                if (progress.Data.NextReviewDate > now)
                    continue;

                foreach (var category in categories.Values)
                {
                    var wordMatch =
                        category.Data.Words.FirstOrDefault(w => w.Id == progress.Data.WordId);
                    if (wordMatch != null)
                        wordsForReview.Add(Entities[wordMatch.Id]);
                }
            }

            return wordsForReview;
        }

        protected override string GetPath(string userId) => $"user_words/{userId}";
    }

    internal sealed class WordEntityData
    {
        internal string CategoryId { get; set; }
        internal string NativeWord { get; set; }
        internal string LearningWord { get; set; }
        internal string Transcription { get; set; }
        internal List<Example> Examples { get; set; } = new();
        internal bool IsDefault { get; set; }

        internal abstract class Example
        {
            internal string NativeSentence { get; set; }
            internal string LearningSentence { get; set; }
        }
    }
}