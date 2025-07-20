using R3;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class NewWordsCompleteBehaviour : LearningCompleteBehaviourBase
    {
        protected override void InitExitButton()
        {
            exitButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);
        }
    }
}