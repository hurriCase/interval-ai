using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Swipe;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice
{
    internal sealed class PracticeBehaviour : MonoBehaviour
    {
        [SerializeField] private PracticeState _practiceState;

        [SerializeField] private CardBehaviour _cardBehaviour;

        [SerializeField] private ProgressIndicatorBehaviour _progressIndicatorBehaviour;

        [SerializeField] private SwipeCardBehaviour _swipeCardBehaviour;

        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        [SerializeField] private LearningCompleteBehaviourBase _learningCompleteBehaviour;

        [Inject] private IWordsRepository _wordsRepository;

        internal void Init()
        {
            _wordsRepository.CurrentWordsByState
                .Select(this, (currentWords, self)
                    => currentWords[self._practiceState])
                .Subscribe(this, (currentWord, self) => self.SwitchState(currentWord))
                .RegisterTo(destroyCancellationToken);

            _cardBehaviour.Init(_practiceState);

            _progressIndicatorBehaviour.Init(_practiceState);

            _swipeCardBehaviour.Init(_practiceState);

            _controlButtonsBehaviour.Init(_practiceState);

            _learningCompleteBehaviour.Init(_practiceState);
        }

        private void SwitchState(WordEntry currentWord)
        {
            var isComplete = currentWord == null || currentWord.Cooldown > DateTime.Now;
            _cardBehaviour.SetActive(isComplete is false);

            _progressIndicatorBehaviour.SetActive(isComplete is false);

            _swipeCardBehaviour.SetActive(isComplete is false);

            _controlButtonsBehaviour.SetActive(isComplete is false);

            _learningCompleteBehaviour.SetActive(isComplete);
        }
    }
}