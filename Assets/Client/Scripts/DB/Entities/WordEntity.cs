using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.DB.Base;

namespace Client.Scripts.DB.Entities
{
    internal sealed class WordEntity : DBEntityBase<WordEntityData>
    {
        internal List<EntityData<WordEntityData>> GetWordsForReview(
            ConcurrentDictionary<string, EntityData<ProgressEntityData>> userProgress,
            ConcurrentDictionary<string, EntityData<CategoryEntityData>> categories)
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

        protected override string GetPath() => "user_words";
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