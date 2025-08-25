using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using JetBrains.Annotations;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;
using Source.Scripts.Core.Repositories.Words.Word;
using ZLinq;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsTimerService : IWordsTimerService, IDisposable
    {
        public Observable<CooldownByPracticeState> OnAvailabilityTimeUpdate => _availabilityTimeSubject.AsObservable();

        private readonly Subject<CooldownByPracticeState> _availabilityTimeSubject = new();
        private EnumArray<PracticeState, AdaptiveTimer?> _stateTimers = new(EnumMode.SkipFirst);

        private readonly ICurrentWordsService _currentWordsService;
        private readonly IAppConfig _appConfig;

        private readonly IDisposable _disposable;

        internal WordsTimerService(ICurrentWordsService currentWordsService, IAppConfig appConfig)
        {
            _currentWordsService = currentWordsService;
            _appConfig = appConfig;

            _disposable = _currentWordsService.CurrentWordsByState
                .Subscribe(this, (_, self) => self.UpdateTimers());
        }

        public void UpdateTimers()
        {
            foreach (var (practiceState, currentWord) in _currentWordsService.CurrentWordsByState.CurrentValue.AsTuples())
                UpdateTimerForPractice(practiceState, currentWord);
        }

        private void UpdateTimerForPractice(PracticeState practiceState, [CanBeNull] WordEntry currentWord)
        {
            if (currentWord is null
                || _appConfig.CooldownStates.AsValueEnumerable().Contains(currentWord.LearningState) is false)
            {
                DisposeTimer(practiceState);
                return;
            }

            if (_stateTimers[practiceState].HasValue)
            {
                _stateTimers[practiceState].Value.UpdateTargetTime(currentWord.Cooldown);
                return;
            }

            _stateTimers[practiceState] = new AdaptiveTimer(currentWord.Cooldown);

            _stateTimers[practiceState].Value.TimeUpdates
                .Subscribe((self: this, practiceState),
                    static (currentTime, tuple) => tuple.self._availabilityTimeSubject
                        .OnNext(new CooldownByPracticeState(tuple.practiceState, currentTime)),
                    static (_, tuple)
                        => tuple.self._currentWordsService.UpdateCurrentWords());
        }

        private void DisposeTimer(PracticeState practiceState)
        {
            _stateTimers[practiceState]?.Dispose();
            _stateTimers[practiceState] = null;
        }

        public void Dispose()
        {
            foreach (var timer in _stateTimers)
                timer?.Dispose();

            _availabilityTimeSubject.Dispose();
            _disposable.Dispose();
        }
    }
}