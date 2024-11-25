using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Scripts.Database.Vocabulary
{
    internal sealed class WordEntityData
    {
        public string CategoryId { get; set; }
        public string NativeWord { get; set; }
        public string LearningWord { get; set; }
        public string Transcription { get; set; }
        public List<Example> Examples { get; set; } = new();
        public bool IsDefault { get; set; }

        public sealed class Example
        {
            public string NativeSentence { get; set; }
            public string LearningSentence { get; set; }
        }
    }

    internal sealed class WordEntity : DataBaseEntity<WordEntityData>, IInitializable
    {
        protected override string GetPath(string userId) => $"user_words/{userId}";

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
    }
}