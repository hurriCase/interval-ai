using R3;
using Source.Scripts.Core.Localization;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        protected override void OnInit()
        {
            exitButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);
        }
    }
}