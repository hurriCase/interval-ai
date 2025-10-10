using System;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class CompleteStateService : ICompleteStateService, IDisposable
    {
        public ReadOnlyReactiveProperty<CompleteType> CompleteStates => _completeStates;
        private readonly ReactiveProperty<CompleteType> _completeStates = new();

        private readonly IDisposable _disposable;

        private readonly ICurrentWordsService _currentWordsService;
        private readonly PracticeState _practiceState;

        internal CompleteStateService(
            ICurrentWordsService currentWordsService,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService,
            PracticeState practiceState)
        {
            _currentWordsService = currentWordsService;
            _practiceState = practiceState;

            var currentWordsDisposable = _currentWordsService.CurrentWordsByState
                .Select(_practiceState, static (currentWords, practiceState) => currentWords[practiceState])
                .Subscribe(this, static (currentWords, self) => self.CheckCompleteness(currentWords));

            var dailyTargetDisposable = progressRepository.HasDailyTarget
                .Where(practiceState, static (_, state) => state == PracticeState.NewWords)
                .Subscribe(this, static (hasTarget, self) => self.CheckCompleteness(hasTarget is false));

            var timeUpdateDisposable = wordsTimerService.OnAvailabilityTimeUpdated
                .Where(practiceState, static (_, state) => state == PracticeState.Review)
                .Subscribe(this, static (cooldown, self)
                    => self.CheckCompleteness(cooldown > DateTime.Now));

            _disposable = Disposable.Combine(currentWordsDisposable, dailyTargetDisposable, timeUpdateDisposable);
        }

        private void CheckCompleteness(bool isComplete)
        {
            if (isComplete)
            {
                _completeStates.Value = CompleteType.Complete;
                return;
            }

            var currentWord = _currentWordsService.CurrentWordsByState.CurrentValue[_practiceState];
            CheckCompleteness(currentWord);
        }

        private void CheckCompleteness(WordEntry wordEntry)
        {
            if (wordEntry == null)
            {
                _completeStates.Value = CompleteType.NoWords;
                return;
            }

            _completeStates.Value = CompleteType.None;
        }

        public void Dispose()
        {
            _completeStates.Dispose();
            _disposable.Dispose();
        }
    }
}