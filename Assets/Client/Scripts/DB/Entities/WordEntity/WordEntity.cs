using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.DB.Entities.Base;
using Client.Scripts.DB.Entities.CategoryEntity;
using Client.Scripts.DB.Entities.ProgressEntity;

namespace Client.Scripts.DB.Entities.WordEntity
{
    internal sealed class WordEntity : EntityBase<WordEntryContent>
    {
        protected override string EntityPath => "word_entity";

        internal List<EntryData<WordEntryContent>> GetWordsForReview(
            ConcurrentDictionary<string, EntryData<ProgressEntryContent>> userProgress,
            ConcurrentDictionary<string, EntryData<UserCategoryEntryContent>> categories)
        {
            var now = DateTime.UtcNow;
            var wordsForReview = new List<EntryData<WordEntryContent>>();

            foreach (var progress in userProgress.Values)
            {
                if (progress.Content.NextReviewDate > now)
                    continue;

                foreach (var category in categories.Values)
                {
                    var wordMatch =
                        category.Content.Words.FirstOrDefault(w => w.Id == progress.Content.WordId);
                    if (wordMatch != null)
                        wordsForReview.Add(Entries[wordMatch.Id]);
                }
            }

            return wordsForReview;
        }
    }
}