using System;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Timer;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Swipe;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class PracticeBehaviour : MonoBehaviour
    {
        [SerializeField] private PracticeState _practiceState;

        [SerializeField] private CardBehaviour _cardBehaviour;

        [SerializeField] private ProgressIndicatorBehaviour _progressIndicatorBehaviour;

        [SerializeField] private SwipeCardBehaviour _swipeCardBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        [SerializeField] private LearningCompleteBehaviourBase _learningCompleteBehaviour;

        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IWordsTimerService _wordsTimerService;

        internal void Init()
        {
            _cardBehaviour.Init();

            _progressIndicatorBehaviour.Init(_practiceState);

            _swipeCardBehaviour.Init(_practiceState);
            _wordProgressBehaviour.Init(_practiceState);

            _controlButtonsBehaviour.Init(_practiceState);

            _learningCompleteBehaviour.Init(_practiceState);

            if (_practiceState == PracticeState.Review)
                _wordsTimerService.OnAvailabilityTimeUpdate
                    .Where(cooldown => cooldown.State == LearningState.Repeatable)
                    .Where(cooldown => DateTime.Now >= cooldown.CurrentTime)
                    .Subscribe(this, static (_, self) => self.UpdateWord())
                    .RegisterTo(destroyCancellationToken);

            _wordsRepository.CurrentWordsByState
                .Select(_practiceState, (currentWordsByState, state)
                    => currentWordsByState[state])
                .DistinctUntilChanged()
                .Subscribe(_cardBehaviour,
                    static (currentWord, cardBehaviour) => cardBehaviour.WordEntry.Value = currentWord)
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateWord()
        {
            _cardBehaviour.WordEntry.Value = _wordsRepository.CurrentWordsByState.Value[_practiceState];
        }
    }
}