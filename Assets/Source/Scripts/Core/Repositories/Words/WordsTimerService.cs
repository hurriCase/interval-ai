using System;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsTimerService : IWordsTimerService, IDisposable
    {
        public Observable<DateTime> OnAvailabilityTimeUpdate => _availabilityTimeSubject.AsObservable();

        private readonly Subject<DateTime> _availabilityTimeSubject = new();
        private AdaptiveTimer? _stateTimers;

        private readonly ICurrentWordsService _currentWordsService;

        private readonly IDisposable _disposable;

        internal WordsTimerService(ICurrentWordsService currentWordsService)
        {
            _currentWordsService = currentWordsService;

            _disposable = _currentWordsService.CurrentWordsByState
                .Subscribe(this, (_, self) => self.UpdateTimer());
        }

        public void UpdateTimer()
        {
            var currentWord = _currentWordsService.CurrentWordsByState.CurrentValue[PracticeState.Review];

            if (currentWord is null || currentWord.LearningState != LearningState.Review)
            {
                DisposeTimer();
                return;
            }

            if (_stateTimers.HasValue)
            {
                _stateTimers.Value.UpdateTargetTime(currentWord.Cooldown);
                return;
            }

            _stateTimers = new AdaptiveTimer(currentWord.Cooldown);

            _stateTimers.Value.TimeUpdates.Subscribe(this,
                static (currentTime, self) => self._availabilityTimeSubject.OnNext(currentTime),
                static (_, self) => self._currentWordsService.UpdateCurrentWords());
        }

        private void DisposeTimer()
        {
            _stateTimers?.Dispose();
            _stateTimers = null;
        }

        public void Dispose()
        {
            _stateTimers?.Dispose();
            _availabilityTimeSubject.Dispose();
            _disposable.Dispose();
        }
    }
}