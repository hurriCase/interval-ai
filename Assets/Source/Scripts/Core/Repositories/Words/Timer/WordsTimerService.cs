using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words.Timer
{
    internal sealed class WordsTimerService : IWordsTimerService, IDisposable
    {
        public Observable<CooldownByLearningState> OnAvailabilityTimeUpdate => _availabilityTimeSubject.AsObservable();

        private readonly Subject<CooldownByLearningState> _availabilityTimeSubject = new();
        private EnumArray<LearningState, AdaptiveTimer> _stateTimers = new(EnumMode.Default);

        private IWordsRepository _wordsRepository;

        public void Init(IWordsRepository wordsRepository)
        {
            _wordsRepository = wordsRepository;

            foreach (var (state, _) in _stateTimers.AsTuples())
            {
                var sortedWords = wordsRepository.SortedWordsByState.Value[state];
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

        public void UpdateTimerForState(LearningState learningState)
        {
            var sortedWords = _wordsRepository.SortedWordsByState.Value[learningState];
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
        }
    }
}