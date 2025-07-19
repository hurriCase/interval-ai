using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal sealed class VocabularyRepository : Singleton<VocabularyRepository>, IDisposable
    {
        internal PersistentReactiveProperty<List<WordEntry>> WordEntries { get; } =
            new(PersistentPropertyKeys.WordEntryKey, DefaultWordsDatabase.Instance.WordEntries);

        internal PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; } =
            new(PersistentPropertyKeys.CategoryEntryKey, DefaultCategoriesDatabase.Instance.Categories);

        internal WordEntry GetAvailableWord(LearningState learningState)
        {
            foreach (var wordEntry in WordEntries.Value)
            {
                if (wordEntry.Cooldown <= DateTime.Now
                    && wordEntry.LearningState == learningState
                    && wordEntry.IsHidden is false)
                    return wordEntry;
            }

            return null;
        }

        internal void AdvanceWord(WordEntry word, bool success)
        {
            if (success)
            {
                HandleSuccess(word);
                return;
            }

            HandleFailure(word);
        }

        private void HandleSuccess(WordEntry word)
        {
            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.CurrentlyLearning;
                    break;

                case LearningState.CurrentlyLearning:
                    word.LearningState = LearningState.Repeatable;
                    word.RepetitionCount = 0;
                    AdvanceCooldown(word);
                    break;

                case LearningState.Repeatable:
                    if (word.RepetitionCount >= UserRepository.Instance.RepetitionByCooldown.Value.Count)
                    {
                        word.LearningState = LearningState.Studied;
                        break;
                    }

                    word.RepetitionCount++;
                    AdvanceCooldown(word);
                    break;

                case LearningState.AlreadyKnown:
                case LearningState.Studied:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            ProgressDataHelper.AddProgressToEntry(word.LearningState, DateTime.Now);
        }

        private void HandleFailure(WordEntry word)
        {
            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.AlreadyKnown;
                    break;

                case LearningState.Repeatable:
                    word.RepetitionCount = Math.Max(0, word.RepetitionCount - 1);
                    AdvanceCooldown(word);
                    break;

                case LearningState.AlreadyKnown:
                case LearningState.CurrentlyLearning:
                case LearningState.Studied:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AdvanceCooldown(WordEntry word)
        {
            if (word.RepetitionCount >= UserRepository.Instance.RepetitionByCooldown.Value.Count)
                return;

            var cooldownData = UserRepository.Instance.RepetitionByCooldown.Value[word.RepetitionCount];
            word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);
        }

        public void Dispose()
        {
            WordEntries.Dispose();
            CategoryEntries.Dispose();
        }
    }
}