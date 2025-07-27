using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        [SerializeField] private ButtonComponent _repeatWordsButton;

        protected override void OnInit()
        {
            _repeatWordsButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);
        }
    }
}