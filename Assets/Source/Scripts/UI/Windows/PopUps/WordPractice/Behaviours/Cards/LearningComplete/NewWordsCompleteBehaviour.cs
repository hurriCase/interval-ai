using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private TextMeshProUGUI _goalAchievedText;

        protected override void OnInit()
        {
            _goalAchievedText.text =
                string.Format(LocalizationController.Localize("ui.word-practice.goal-achieved"),
                    ProgressRepository.Instance.NewWordsCount);

            exitButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);
        }
    }
}