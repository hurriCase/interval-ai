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
                .Subscribe(practiceStateService, static (_, service) => service.SetState(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);

            _progressRepository.GoalAchievedObservable
                .Subscribe(this,
                    static (wordsCount, self) => self.SetState(CompleteType.Complete, wordsCount.ToString()))
                .RegisterTo(destroyCancellationToken);
        }
    }
}