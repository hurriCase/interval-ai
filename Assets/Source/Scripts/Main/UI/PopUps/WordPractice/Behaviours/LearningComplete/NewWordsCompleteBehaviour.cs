using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private ButtonComponent _repeatWordsButton;

        [Inject] private IProgressRepository _progressRepository;

        protected override void OnInit()
        {
            _repeatWordsButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.CurrentState.OnNext(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);

            _progressRepository.GoalAchievedObservable
                .Subscribe(this, static (_, self) => self.ShowGoalAchieved())
                .RegisterTo(destroyCancellationToken);
        }

        private void ShowGoalAchieved()
        {
            // learningCompleteBehaviour.SetState(CompleteType.NoWords, progressRepository.NewWordsCount.ToString());
            // cardContainer.SetActive(false);
            // learningCompleteBehaviour.SetActive(true);
        }
    }
}