using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Onboarding.Data.Config;

namespace Source.Scripts.Onboarding.Data.CurrentWords
{
    internal sealed class OnboardingCurrentWordsService : ICurrentWordsService, IDisposable
    {
        public ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState =>
            _currentWordsByState;

        private readonly ReactiveProperty<EnumArray<PracticeState, WordEntry>> _currentWordsByState
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private readonly DefaultOnboardingDatabase _onboardingDatabase;
        private readonly PracticeState _onboardingPracticeState;

        private int _currentWordIndex;

        public OnboardingCurrentWordsService(
            DefaultOnboardingDatabase onboardingDatabase,
            OnboardingConfig onboardingConfig)
        {
            _onboardingDatabase = onboardingDatabase;
            _onboardingPracticeState = onboardingConfig.OnboardingPracticeState;
        }

        public void SetCurrentWord(PracticeState practiceState, WordEntry word)
        {
            var currentWords = _currentWordsByState.Value;
            currentWords[_onboardingPracticeState] = word;
            _currentWordsByState.Value = currentWords;
            _currentWordsByState.OnNext(currentWords);
        }

        public void UpdateCurrentWords()
        {
            if (_currentWordIndex >= _onboardingDatabase.Defaults.Count)
                return;

            var currentWord = _onboardingDatabase.Defaults[_currentWordIndex];

            SetCurrentWord(_onboardingPracticeState, currentWord);

            _currentWordIndex++;
        }

        public bool HasWordByState(PracticeState practiceState)
            => _currentWordsByState.CurrentValue[_onboardingPracticeState] != null;

        public void Dispose()
        {
            _currentWordsByState?.Dispose();
        }
    }
}