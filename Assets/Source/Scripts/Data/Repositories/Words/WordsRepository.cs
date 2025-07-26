using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using R3;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Data.Repositories.Words.Data;
using ZLinq;
using ZLinq.Linq;
using Random = UnityEngine.Random;

namespace Source.Scripts.Data.Repositories.Words
{
    internal sealed class WordsRepository : IWordsRepository, IDisposable
    {
        private PersistentReactiveProperty<List<WordEntry>> WordEntries { get; }

        private EnumArray<LearningState, SortedSet<WordEntry>> SortedWordsByState { get; }

        private readonly Subject<CooldownByLearningState> _availabilityTimeSubject = new();
        public Observable<CooldownByLearningState> OnAvailabilityTimeUpdate => _availabilityTimeSubject.AsObservable();

        private EnumArray<LearningState, AdaptiveTimer> _stateTimers = new(EnumMode.SkipFirst);

        private static readonly WordCooldownComparer _comparer = new();

        private readonly IProgressRepository _progressRepository;
        private readonly ISettingsRepository _settingsRepository;

        internal WordsRepository(
            IProgressRepository progressRepository,
            ISettingsRepository settingsRepository,
            IDefaultWordsDatabase defaultWordsDatabase)
        {
            WordEntries = new PersistentReactiveProperty<List<WordEntry>>(PersistentPropertyKeys.WordEntryKey,
                defaultWordsDatabase.WordEntries);
            SortedWordsByState =
                new EnumArray<LearningState, SortedSet<WordEntry>>(() => new SortedSet<WordEntry>(_comparer));

            _progressRepository = progressRepository;
            _settingsRepository = settingsRepository;

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

        //TODO:<Dmitriy.Sukharev> Refactor
        public ValueEnumerable<OrderBySkipTake<ListWhere<WordEntry>, WordEntry, float>, WordEntry>
            GetRandomWords(WordEntry wordToSkip, int count) =>
            WordEntries.Value.AsValueEnumerable()
                .Where(word => word != wordToSkip && word.IsHidden is false)
                .OrderBy(_ => Random.value)
                .Take(count);

        public void AdvanceWord(WordEntry word, bool success)
        {
            if (success)
                HandleSuccess(word);
            else
                HandleFailure(word);

            WordEntries.SaveAsync();
        }

        private void HandleSuccess(WordEntry word)
        {
            SortedWordsByState[word.LearningState].Remove(word);

            switch (word.LearningState)
            {
                case LearningState.None:
                    word.LearningState = LearningState.CurrentlyLearning;
                    break;

                case LearningState.CurrentlyLearning:
                    word.LearningState = LearningState.Repeatable;
                    word.RepetitionCount = 0;
                    _progressRepository.NewWordsDailyTarget.Value--;

                    AdvanceCooldown(word);

                    _progressRepository.IncrementNewWordsCount();
                    break;

                case LearningState.Repeatable:
                    _progressRepository.IncrementReviewCount();

                    if (word.RepetitionCount >= _settingsRepository.RepetitionByCooldown.Value.Count)
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

            SortedWordsByState[word.LearningState].Add(word);
        }

        private void HandleFailure(WordEntry word)
        {
            SortedWordsByState[word.LearningState].Remove(word);

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

            SortedWordsByState[word.LearningState].Add(word);
        }

        private void AdvanceCooldown(WordEntry word)
        {
            var oldState = word.LearningState;

            if (word.RepetitionCount >= _settingsRepository.RepetitionByCooldown.Value.Count)
                return;

            var cooldownData = _settingsRepository.RepetitionByCooldown.Value[word.RepetitionCount];
            word.Cooldown = cooldownData.AddToDateTime(DateTime.Now);

            UpdateTimerForState(oldState);
            UpdateTimerForState(word.LearningState);

            WordEntries.SaveAsync();
        }

        private void UpdateTimerForState(LearningState learningState)
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