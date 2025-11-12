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
    internal sealed class CurrentWordService : ICurrentWordService, IDisposable
    {
        public ReadOnlyReactiveProperty<WordEntry> CurrentWord => _currentWord;
        private readonly ReactiveProperty<WordEntry> _currentWord = new();

        private int _currentWordIndex;

        private readonly DefaultWordsDatabase _wordsDatabase;
        private readonly IPracticeStateService _practiceStateService;

        [Preserve]
        internal CurrentWordService(
            DefaultWordsDatabase wordsDatabase,
            IPracticeStateService practiceStateService)
        {
            _wordsDatabase = wordsDatabase;
            _practiceStateService = practiceStateService;
        }

        public void SetCurrentWord(WordEntry word)
        {
            _currentWord.Value = word;
            _currentWord.OnNext(word);
        }

        public void UpdateCurrentWord()
        {
            if (_currentWordIndex >= _wordsDatabase.Defaults.Count)
                return;

            var currentWord = _wordsDatabase.Defaults[_currentWordIndex];

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