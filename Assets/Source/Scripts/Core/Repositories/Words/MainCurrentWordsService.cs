using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words
{
    internal sealed class MainCurrentWordsService : ICurrentWordsService, IDisposable
    {
        public ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState =>
            _currentWordsByState;

        private readonly ReactiveProperty<EnumArray<PracticeState, WordEntry>> _currentWordsByState
            = new(new EnumArray<PracticeState, WordEntry>(EnumMode.SkipFirst));

        private readonly IProgressRepository _progressRepository;
        private readonly IWordsRepository _wordsRepository;
        private readonly IAppConfig _appConfig;

        internal MainCurrentWordsService(
            IProgressRepository progressRepository,
            IWordsRepository wordsRepository,
            IAppConfig appConfig)
        {
            _progressRepository = progressRepository;
            _wordsRepository = wordsRepository;
            _appConfig = appConfig;

            _wordsRepository.SortedWordsByState.Subscribe(this, (_, self) => self.UpdateCurrentWords());
        }

        public void SetCurrentWord(PracticeState practiceState, WordEntry word)
        {
            var currentWordsByState = _currentWordsByState.Value;
            currentWordsByState[practiceState] = word;
            _currentWordsByState.Value = currentWordsByState;
            _currentWordsByState.OnNext(currentWordsByState);
        }

        public void UpdateCurrentWords()
        {
            foreach (var (practiceState, learningStates) in
                     _appConfig.TargetLearningStatesForPractice.AsTuples())
            {
                foreach (var learningState in learningStates)
                {
                    var nearestWord = _wordsRepository.SortedWordsByState.CurrentValue[learningState].Min;
                    if (nearestWord != null && CheckDailyComplete(nearestWord.LearningState) is false)
                        continue;

                    SetCurrentWord(practiceState, nearestWord);

                    if (nearestWord != null)
                        break;
                }
            }
        }

        private bool CheckDailyComplete(LearningState learningState)
            => LearningState.Default != learningState || _progressRepository.HasDailyTarget.CurrentValue;

        public bool HasWordByState(PracticeState practiceState)
            => CurrentWordsByState.CurrentValue[practiceState] != null;

        public void Dispose()
        {
            _currentWordsByState.Dispose();
        }
    }
}