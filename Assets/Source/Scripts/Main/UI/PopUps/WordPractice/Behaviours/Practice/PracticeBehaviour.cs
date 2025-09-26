using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
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

        private WordEntry CurrentWord => _currentWordsService.CurrentWordsByState.CurrentValue[_practiceState];

        private ICompleteStateService _completeStateService;
        private ICurrentWordsService _currentWordsService;
        private IWordAdvanceService _wordAdvanceService;

        [Inject]
        public void Inject(
            ICompleteStateService completeStateService,
            ICurrentWordsService currentWordsService,
            IWordAdvanceService wordAdvanceService)
        {
            _completeStateService = completeStateService;
            _currentWordsService = currentWordsService;
            _wordAdvanceService = wordAdvanceService;
        }

        internal void Init()
        {
            _cardBehaviour.Init(_practiceState);
            _progressIndicatorBehaviour.Init(_practiceState);
            _swipeCardBehaviour.Init();
            _controlButtonsBehaviour.Init(_practiceState);
            _learningCompleteBehaviour.Init(_practiceState);

            _completeStateService.CompleteStates
                .Select(_practiceState, (completeTypes, state) => completeTypes[state])
                .SubscribeUntilDestroy(this, static (completeType, self) => self.SwitchState(completeType));

            _swipeCardBehaviour.OnSwiped
                .SubscribeUntilDestroy(this, static (direction, self) => self.HandleSwipe(direction));
        }

        private void SwitchState(CompleteType completeType)
        {
            var isComplete = completeType != CompleteType.None;

            _cardBehaviour.SetActive(isComplete is false);
            _progressIndicatorBehaviour.SetActive(isComplete is false);
            _swipeCardBehaviour.SetActive(isComplete is false);
            _controlButtonsBehaviour.SetActive(isComplete is false);

            _learningCompleteBehaviour.SetActive(isComplete);
        }

        private void HandleSwipe(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                    _wordAdvanceService.AdvanceWord(
                        CurrentWord,
                        _currentWordsService.IsFirstShow(_practiceState));
                    break;

                case SwipeDirection.Right:
                    _wordAdvanceService.AdvanceWord(
                        CurrentWord,
                        _currentWordsService.IsFirstShow(_practiceState) is false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }
    }
}