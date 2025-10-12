using System;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEngine.Scripting;

namespace Source.Scripts.Main.Data.CurrentWord
{
    [Preserve]
    internal sealed class CurrentWordService : ICurrentWordService, IDisposable
    {
        public ReadOnlyReactiveProperty<WordEntry> CurrentWord => _currentWord;

        private readonly ReactiveProperty<WordEntry> _currentWord = new();

        private readonly IProgressRepository _progressRepository;
        private readonly IWordsRepository _wordsRepository;
        private readonly IAppConfig _appConfig;
        private readonly PracticeState _practiceState;

        [Preserve]
        internal CurrentWordService(
            IProgressRepository progressRepository,
            IWordsRepository wordsRepository,
            IAppConfig appConfig,
            PracticeState practiceState)
        {
            _progressRepository = progressRepository;
            _wordsRepository = wordsRepository;
            _appConfig = appConfig;
            _practiceState = practiceState;

            _wordsRepository.SortedWordsByState.Subscribe(this, (_, self) => self.UpdateCurrentWord());
        }

        public void UpdateCurrentWord()
        {
            foreach (var learningState in _appConfig.TargetLearningStatesForPractice[_practiceState])
            {
                var nearestWord = _wordsRepository.SortedWordsByState.CurrentValue[learningState].Min;
                if (nearestWord != null && CheckDailyComplete(nearestWord.LearningState) is false)
                    continue;

                SetCurrentWord(nearestWord);

                if (nearestWord != null)
                    break;
            }
        }

        public void SetCurrentWord(WordEntry word)
        {
            _currentWord.Value = word;
            _currentWord.OnNext(word);
        }

        public bool HasWord() => CurrentWord != null;

        public bool IsFirstShow()
            => _currentWord.Value?.LearningState == LearningState.Default && _practiceState == PracticeState.NewWords;

        private bool CheckDailyComplete(LearningState learningState)
            => LearningState.Default != learningState || _progressRepository.HasDailyTarget.CurrentValue;

        public void Dispose()
        {
            _currentWord.Dispose();
        }
    }
}