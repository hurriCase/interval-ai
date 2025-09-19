using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class CompleteStateService : ICompleteStateService, IDisposable
    {
        public ReadOnlyReactiveProperty<EnumArray<PracticeState, CompleteType>> CompleteStates => _completeStates;
        private readonly ReactiveProperty<EnumArray<PracticeState, CompleteType>> _completeStates
            = new(new EnumArray<PracticeState, CompleteType>(EnumMode.SkipFirst));

        private readonly ICurrentWordsService _currentWordsService;

        private readonly IDisposable _disposable;

        internal CompleteStateService(
            ICurrentWordsService currentWordsService,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService)
        {
            _currentWordsService = currentWordsService;

            var builder = Disposable.CreateBuilder();

            _currentWordsService.CurrentWordsByState
                .Subscribe(this, static (_, self) => self.CheckCompleteness())
                .AddTo(ref builder);

            progressRepository.HasDailyTarget
                .Subscribe(this, static (hasTarget, self)
                    => self.CheckCompleteness(PracticeState.NewWords, hasTarget is false))
                .AddTo(ref builder);

            wordsTimerService.OnAvailabilityTimeUpdated
                .Subscribe(this, static (cooldown, self)
                    => self.CheckCompleteness(PracticeState.Review, cooldown > DateTime.Now))
                .AddTo(ref builder);

            _disposable = builder.Build();
        }

        private void CheckCompleteness(PracticeState practiceState, bool isComplete)
        {
            if (isComplete)
            {
                SetState(practiceState, CompleteType.Complete);
                return;
            }

            var currentWord = _currentWordsService.CurrentWordsByState.CurrentValue[practiceState];
            CheckCompleteness(practiceState, currentWord);
        }

        private void SetState(PracticeState practiceState, CompleteType completeType)
        {
            var completeStatesValue = _completeStates.Value;
            completeStatesValue[practiceState] = completeType;
            _completeStates.Value = completeStatesValue;
            _completeStates.OnNext(completeStatesValue);
        }

        private void CheckCompleteness()
        {
            var currentWords = _currentWordsService.CurrentWordsByState.CurrentValue;
            foreach (var (practiceState, wordEntry) in currentWords.AsTuples())
                CheckCompleteness(practiceState, wordEntry);
        }

        private void CheckCompleteness(PracticeState practiceState, WordEntry wordEntry)
        {
            if (wordEntry == null)
            {
                SetState(practiceState, CompleteType.NoWords);
                return;
            }

            SetState(practiceState, CompleteType.None);
        }

        public void Dispose()
        {
            _completeStates.Dispose();
            _disposable.Dispose();
        }
    }
}