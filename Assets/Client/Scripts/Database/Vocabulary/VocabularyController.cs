using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Database.Vocabulary
{
    internal static class VocabularyController
    {
        private static IDBController _dbController;
        private static Dictionary<string, Category> _categories;
        private static Dictionary<string, WordProgress> _userProgress;
        private static string _userId;

        internal static async Task Init(IDBController dbController, string userId)
        {
            _dbController = dbController;
            _categories = new Dictionary<string, Category>();
            _userProgress = new Dictionary<string, WordProgress>();
            _userId = userId;

            await LoadCategories();
            await LoadUserProgress();
        }

        internal static async Task<Category> CreateCategory(string title, string description, Image icon)
        {
            var category = new Category
            {
                title = title,
                description = description,
                icon = icon,
                words = new Dictionary<string, Word>(),
                isDefault = false
            };

            _categories[category.id] = category;

            await _dbController.WriteData(category.GetPath(_userId), category);

            return category;
        }

        internal static async Task<Word> AddWord(
            string nativeWord, string learningWord, string transcription, List<Word.Example> examples,
            string categoryId)
        {
            if (_categories.TryGetValue(categoryId, out var category) is false)
                throw new KeyNotFoundException("Category not found");

            var word = new Word
            {
                categoryId = categoryId,
                nativeWord = nativeWord,
                learningWord = learningWord,
                transcription = transcription,
                examples = examples,
                isDefault = false
            };

            category.words[word.id] = word;

            await _dbController.WriteData(word.GetPath(_userId), word);

            return word;
        }

        internal static async Task UpdateWordProgress(string wordId, bool wasCorrect)
        {
            if (_userProgress.TryGetValue(wordId, out var progress) is false)
            {
                progress = new WordProgress
                {
                    wordId = wordId
                };

                _userProgress[wordId] = progress;
            }

            progress.totalReviews++;

            if (wasCorrect)
            {
                progress.correctReviews++;
                progress.repetitionStage = Math.Min(5, progress.repetitionStage + 1);
            }
            else
            {
                progress.repetitionStage = Math.Max(0, progress.repetitionStage - 1);
            }

            progress.lastReviewDate = DateTime.UtcNow;
            progress.nextReviewDate = CalculateNextReviewDate(progress.repetitionStage);
            progress.updatedAt = DateTime.UtcNow;

            await _dbController.WriteData(progress.GetPath(_userId), progress);
        }

        internal static List<Word> GetWordsForReview()
        {
            var now = DateTime.UtcNow;
            var wordsForReview = new List<Word>();

            foreach (var progress in _userProgress.Values)
            {
                if (progress.nextReviewDate > now)
                    continue;

                foreach (var category in _categories.Values)
                {
                    if (category.words.TryGetValue(progress.wordId, out var word) is false)
                        continue;

                    wordsForReview.Add(word);
                }
            }

            return wordsForReview;
        }

        private static DateTime CalculateNextReviewDate(int repetitionStage)
        {
            //TODO: Change logic
            var hoursToAdd = repetitionStage switch
            {
                0 => 1, // 1 hour
                1 => 3, // 3 hours
                2 => 8, // 8 hours
                3 => 24, // 1 day
                4 => 72, // 3 days
                5 => 168, // 1 week
                _ => 1
            };

            return DateTime.UtcNow.AddHours(hoursToAdd);
        }

        private static async Task LoadCategories()
        {
            try
            {
                var globalCategories = await _dbController.ReadData<Dictionary<string, Category>>("global_categories");
                if (globalCategories != null)
                {
                    foreach (var category in globalCategories)
                    {
                        _categories[category.Key] = category.Value;
                    }
                }

                var userCategories =
                    await _dbController.ReadData<Dictionary<string, Category>>($"user_categories/{_userId}");
                if (userCategories != null)
                {
                    foreach (var category in userCategories)
                    {
                        _categories[category.Key] = category.Value;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading categories: {e.Message}");
            }
        }

        private static async Task LoadUserProgress()
        {
            try
            {
                var progress =
                    await _dbController.ReadData<Dictionary<string, WordProgress>>($"users/{_userId}/progress");
                if (progress != null)
                {
                    foreach (var item in progress)
                    {
                        _userProgress[item.Key] = item.Value;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading user progress: {e.Message}");
            }
        }
    }
}