using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Onboarding.Data.CurrentWords
{
    internal sealed class OnboardingCurrentWordsService : ICurrentWordsService, IDisposable
    {
        public ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState =>
            _currentWordsByState;

        private readonly ReactiveProperty<EnumArray<PracticeState, WordEntry>> _currentWordsByState
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private int _currentWordIndex;

        private readonly DefaultOnboardingDatabase _onboardingDatabase;
        private readonly IPracticeStateService _practiceStateService;

        internal OnboardingCurrentWordsService(
            DefaultOnboardingDatabase onboardingDatabase,
            IPracticeStateService practiceStateService)
        {
            _onboardingDatabase = onboardingDatabase;
            _practiceStateService = practiceStateService;
        }

        public void SetCurrentWord(PracticeState practiceState, WordEntry word)
        {
            var currentWords = _currentWordsByState.Value;
            currentWords[_practiceStateService.CurrentState.CurrentValue] = word;
            _currentWordsByState.Value = currentWords;
            _currentWordsByState.OnNext(currentWords);
        }

        public void UpdateCurrentWords()
        {
            if (_currentWordIndex >= _onboardingDatabase.Defaults.Count)
                return;

            var currentWord = _onboardingDatabase.Defaults[_currentWordIndex];

            SetCurrentWord(_practiceStateService.CurrentState.CurrentValue, currentWord);

            _currentWordIndex++;
        }

        public bool HasWordByState(PracticeState practiceState)
            => _currentWordsByState.CurrentValue[_practiceStateService.CurrentState.CurrentValue] != null;

        public bool IsFirstShow(PracticeState practiceState)
            => _practiceStateService.CurrentState.CurrentValue == PracticeState.NewWords;

        public void Dispose()
        {
            _currentWordsByState?.Dispose();
        }
    }
}