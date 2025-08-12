using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Timer;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private ButtonComponent _returnLateButton;

        [Inject] private IWordsTimerService _wordsTimerService;

        protected override void OnInit()
        {
            _wordsTimerService.OnAvailabilityTimeUpdate
                .Where(cooldownByLearningState => cooldownByLearningState.PracticeState == PracticeState.Review)
                .Subscribe(this, static (cooldownByLearningState, card) => card.SetState(
                    CompleteType.Complete,
                    cooldownByLearningState.CurrentTime.ToShortTimeString())
                )
                .RegisterTo(destroyCancellationToken);

            _returnLateButton.OnClickAsObservable()
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);
        }
    }
}