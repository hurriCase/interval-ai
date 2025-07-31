using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private ButtonComponent _returnLateButton;

        [Inject] private IWordsRepository _wordsRepository;

        protected override void OnInit()
        {
            _wordsRepository.OnAvailabilityTimeUpdate
                .Where(cooldownByLearningState => cooldownByLearningState.State == LearningState.Repeatable)
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