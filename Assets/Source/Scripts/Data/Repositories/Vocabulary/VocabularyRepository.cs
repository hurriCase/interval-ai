using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using R3;
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

        internal PersistentReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>>
            SortedWordsByState { get; } = new(PersistentPropertyKeys.SortedWordsByStateKey,
            new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer)));

        private readonly Subject<(LearningState state, DateTime currentTime)> _availabilityTimeSubject = new();
        public Observable<(LearningState state, DateTime currentTime)> OnAvailabilityTimeUpdate =>
            _availabilityTimeSubject.AsObservable();

        private EnumArray<LearningState, AdaptiveTimer> _stateTimers = new(EnumMode.SkipFirst);

        private static readonly WordCooldownComparer _comparer = new();

        internal void Init()
        {
            foreach (var word in WordEntries.Value)
            {
                var enumArray = SortedWordsByState.Value;
                enumArray[word.LearningState].Add(word);
                SortedWordsByState.Value = enumArray;
            }

            foreach (var (state, _) in _stateTimers.AsTuples())
            {
                var sortedWords = SortedWordsByState.Value[state];
                if (sortedWords.Count == 0)
                    continue;

                _stateTimers[state] = new AdaptiveTimer(sortedWords.Min.Cooldown);

                _stateTimers[state].TimeUpdates.Subscribe((behaviour: this, state),
                    (currentTime, tuple) =>
                        tuple.behaviour._availabilityTimeSubject.OnNext((tuple.state, currentTime)),
                    onCompleted: (_, tuple) => tuple.behaviour.UpdateTimerForState(tuple.state)
                );
            }
        }

        internal WordEntry GetAvailableWord(LearningState learningState)
        {
            if (SortedWordsByState.Value[learningState].Count == 0)
                return null;

            foreach (var word in SortedWordsByState.Value[learningState])
            {
                if (word.Cooldown > DateTime.Now)
                    return null;

                if (word.IsHidden is false)
                    return word;
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
            var oldState = word.LearningState;

            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.CurrentlyLearning;
                    break;

                case LearningState.CurrentlyLearning:
                    word.LearningState = LearningState.Repeatable;
                    word.RepetitionCount = 0;

                    AdvanceCooldown(word);

                    ProgressRepository.Instance.IncrementNewWordsCount();
                    break;

                case LearningState.Repeatable:
                    ProgressRepository.Instance.IncrementReviewCount();

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

            UpdateWordInCollections(word, oldState, word.LearningState);

            ProgressDataHelper.AddProgressToEntry(word.LearningState, DateTime.Now);
        }

        private void HandleFailure(WordEntry word)
        {
            var oldState = word.LearningState;

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

            UpdateWordInCollections(word, oldState, word.LearningState);
        }

        private void AdvanceCooldown(WordEntry word)
        {
            var oldState = word.LearningState;

            if (word.RepetitionCount >= UserRepository.Instance.RepetitionByCooldown.Value.Count)
                return;

            var cooldownData = UserRepository.Instance.RepetitionByCooldown.Value[word.RepetitionCount];
            word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);

            UpdateTimerForState(oldState);
            UpdateTimerForState(word.LearningState);
        }

        private void UpdateWordInCollections(WordEntry word, LearningState oldState, LearningState newState)
        {
            if (oldState == newState)
                return;

            var collections = SortedWordsByState.Value;

            collections[oldState].Remove(word);
            collections[newState].Add(word);

            SortedWordsByState.Value = collections;
        }

        private void UpdateTimerForState(LearningState learningState)
        {
            var sortedWords = SortedWordsByState.Value[learningState];
            if (sortedWords.Count == 0)
                return;

            var earliestWord = sortedWords.Min;

            _stateTimers[learningState].UpdateTargetTime(earliestWord.Cooldown);
        }

        public void Dispose()
        {
            foreach (var adaptiveTimer in _stateTimers)
                adaptiveTimer.Dispose();

            WordEntries.Dispose();
            CategoryEntries.Dispose();
        }
    }
}