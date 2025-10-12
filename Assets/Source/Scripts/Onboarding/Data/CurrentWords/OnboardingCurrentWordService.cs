using System;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEngine.Scripting;

namespace Source.Scripts.Onboarding.Data.CurrentWords
{
    [Preserve]
    internal sealed class OnboardingCurrentWordService : ICurrentWordService, IDisposable
    {
        public ReadOnlyReactiveProperty<WordEntry> CurrentWord => _currentWord;
        private readonly ReactiveProperty<WordEntry> _currentWord = new();

        private int _currentWordIndex;

        private readonly DefaultOnboardingDatabase _onboardingDatabase;
        private readonly IPracticeStateService _practiceStateService;

        [Preserve]
        internal OnboardingCurrentWordService(
            DefaultOnboardingDatabase onboardingDatabase,
            IPracticeStateService practiceStateService)
        {
            _onboardingDatabase = onboardingDatabase;
            _practiceStateService = practiceStateService;
        }

        public void SetCurrentWord(WordEntry word)
        {
            _currentWord.Value = word;
            _currentWord.OnNext(word);
        }

        public void UpdateCurrentWord()
        {
            if (_currentWordIndex >= _onboardingDatabase.Defaults.Count)
                return;

            var currentWord = _onboardingDatabase.Defaults[_currentWordIndex];

            SetCurrentWord(currentWord);

            _currentWordIndex++;
        }

        public bool HasWord() => _currentWord.CurrentValue != null;

        public bool IsFirstShow() => _practiceStateService.CurrentState.CurrentValue == PracticeState.NewWords;

        public void Dispose()
        {
            _currentWord?.Dispose();
        }
    }
}