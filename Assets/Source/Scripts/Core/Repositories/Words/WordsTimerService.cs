using System;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class WordsTimerService : IWordsTimerService, IDisposable
    {
        public Observable<DateTime> OnAvailabilityTimeUpdated => _availabilityTimeUpdated;
        private readonly Subject<DateTime> _availabilityTimeUpdated = new();

        private AdaptiveTimer? _stateTimers;

        private readonly ICurrentWordService _currentWordService;

        private readonly IDisposable _disposable;

        internal WordsTimerService(ICurrentWordFactory currentWordFactory)
        {
            _currentWordService = currentWordFactory.GetOrCreate(PracticeState.Review);

            _disposable = _currentWordService.CurrentWord
                .Subscribe(this, (_, self) => self.UpdateTimer());
        }

        public void UpdateTimer()
        {
            var currentWord = _currentWordService.CurrentWord.CurrentValue;

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

            _stateTimers.Value.OnTimeUpdated.Subscribe(this,
                static (currentTime, self) => self._availabilityTimeUpdated.OnNext(currentTime),
                static (_, self) => self._currentWordService.UpdateCurrentWord());
        }

        private void DisposeTimer()
        {
            _stateTimers?.Dispose();
            _stateTimers = null;
        }

        public void Dispose()
        {
            _stateTimers?.Dispose();
            _availabilityTimeUpdated.Dispose();
            _disposable.Dispose();
        }
    }
}