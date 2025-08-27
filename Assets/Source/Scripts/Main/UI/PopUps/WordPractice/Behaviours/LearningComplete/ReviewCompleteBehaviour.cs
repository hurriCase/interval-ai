using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private GameObject _addNewWordsContainer;
        [SerializeField] private ButtonComponent _exitButton;
        [SerializeField] private ButtonComponent _learnButton;

        [Inject] private IWordsTimerService _wordsTimerService;

        protected override void OnInit()
        {
            positiveButton.Button.OnClickAsObservable()
                .SubscribeAndRegister(this, self => self.SetActiveNewWords(true));

            _learnButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.AddNewWords());

            _wordsTimerService.OnAvailabilityTimeUpdate
                .SubscribeAndRegister(this, static (time, self) =>
                    self.SetState(CompleteType.Complete, time.ToShortTimeString()));

            _exitButton.OnClickAsObservable().SubscribeAndRegister(this, self => self.OpenLearnWords());
            negativeButton.Button.OnClickAsObservable().SubscribeAndRegister(this, self => self.OpenLearnWords());
        }

        private void AddNewWords()
        {
            practiceStateService.SetState(PracticeState.NewWords);

            SetActiveNewWords(false);
        }

        private void OpenLearnWords()
        {
            windowsController.OpenScreenByType(ScreenType.LearningWords);
        }

        private void SetActiveNewWords(bool isActive)
        {
            buttonsContainer.SetActive(isActive is false);
            _addNewWordsContainer.SetActive(isActive);
        }
    }
}