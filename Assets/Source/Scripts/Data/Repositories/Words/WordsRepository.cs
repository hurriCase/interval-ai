using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using R3;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;
using ZLinq;
using Random = UnityEngine.Random;

namespace Source.Scripts.Data.Repositories.Words
{
    internal sealed class WordsRepository : IWordsRepository, IDisposable
    {
        public Observable<CooldownByLearningState> OnAvailabilityTimeUpdate => _availabilityTimeSubject.AsObservable();
        public PersistentReactiveProperty<List<WordEntry>> WordEntries { get; }
        public EnumArray<LearningState, SortedSet<WordEntry>> SortedWordsByState { get; }

        private readonly Subject<CooldownByLearningState> _availabilityTimeSubject = new();
        private EnumArray<LearningState, AdaptiveTimer> _stateTimers = new(EnumMode.SkipFirst);

        private static readonly WordCooldownComparer _comparer = new();

        internal WordsRepository()
        {
            WordEntries = new PersistentReactiveProperty<List<WordEntry>>(PersistentPropertyKeys.WordEntryKey);
            SortedWordsByState =
                new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer));

            foreach (var word in WordEntries.Value)
                SortedWordsByState[word.LearningState].Add(word);

            foreach (var (state, _) in _stateTimers.AsTuples())
            {
                var sortedWords = SortedWordsByState[state];
                if (sortedWords.Count == 0)
                    continue;

                _stateTimers[state] = new AdaptiveTimer(sortedWords.Min.Cooldown);

                _stateTimers[state].TimeUpdates.Subscribe((behaviour: this, state),
                    (currentTime, tuple) =>
                        tuple.behaviour._availabilityTimeSubject.OnNext(
                            new CooldownByLearningState(tuple.state, currentTime)),
                    (_, tuple) => tuple.behaviour.UpdateTimerForState(tuple.state)
                );
            }
        }

        public WordEntry GetAvailableWord(LearningState learningState) =>
            SortedWordsByState[learningState].Count > 0
                ? SortedWordsByState[learningState].AsValueEnumerable().FirstOrDefault(word => word.IsHidden is false)
                : null;

        public List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count) =>
            WordEntries.Value.AsValueEnumerable()
                .Where(word => word != wordToSkip && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(count)
                .ToList();

        public void UpdateTimerForState(LearningState learningState)
        {
            var sortedWords = SortedWordsByState[learningState];
            if (sortedWords.Count == 0)
                return;

            var earliestWord = sortedWords.Min;

            _stateTimers[learningState].UpdateTargetTime(earliestWord.Cooldown);
        }

        public void Dispose()
        {
            foreach (var adaptiveTimer in _stateTimers)
                adaptiveTimer.Dispose();

            _availabilityTimeSubject?.Dispose();
            WordEntries?.Dispose();
        }
    }
}