using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.DB.Base;
using UnityEngine;

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

    [Serializable]
    internal struct WordEntityData
    {
        [field: SerializeField] public string CategoryId { get; set; }
        [field: SerializeField] public string NativeWord { get; set; }
        [field: SerializeField] public string LearningWord { get; set; }
        [field: SerializeField] public string Transcription { get; set; }
        [field: SerializeField] public List<Example> Examples { get; set; }
        [field: SerializeField] public bool IsDefault { get; set; }

        [Serializable]
        internal struct Example
        {
            [field: SerializeField] public string NativeSentence { get; set; }
            [field: SerializeField] public string LearningSentence { get; set; }
        }
    }
}