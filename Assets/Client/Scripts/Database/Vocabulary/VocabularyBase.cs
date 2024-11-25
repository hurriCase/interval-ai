using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Client.Scripts.Database.Vocabulary
{
    internal abstract class VocabularyBase
    {
        internal string id { get; } = Guid.NewGuid().ToString();

        internal DateTime createdAt { get; set; } = DateTime.UtcNow;
        internal DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }

    internal class Category : VocabularyBase
    {
        internal string title { get; set; }
        internal string description { get; set; }
        internal Image icon { get; set; }
        internal Dictionary<string, Word> words { get; set; }
        internal bool isDefault { get; set; }

        internal string GetPath(string userId) => $"{(isDefault ? "global_categories" : "user_categories")}" +
                                                  $"{(isDefault ? string.Empty : "/" + userId)}/" +
                                                  $"{id}";
    }

    internal class Word : VocabularyBase
    {
        internal string categoryId { get; set; }
        internal string nativeWord { get; set; }
        internal string learningWord { get; set; }
        internal string transcription { get; set; }
        internal List<Example> examples { get; set; } = new();

        internal bool isDefault { get; set; }

        internal sealed class Example
        {
            internal string nativeSentence { get; set; }
            internal string learningSentence { get; set; }
        }

        internal string GetPath(string userId) => $"user_words/{userId}/{id}";
    }

    internal class WordProgress : VocabularyBase
    {
        internal string wordId { get; set; }
        internal int repetitionStage { get; set; }
        internal int totalReviews { get; set; }
        internal int correctReviews { get; set; }
        internal DateTime lastReviewDate { get; set; }
        internal DateTime nextReviewDate { get; set; }

        internal string GetPath(string userId) => $"user_progress/{userId}/{wordId}";
    }
}