using R3;
using Source.Scripts.UI.Windows.Base;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal sealed class ReviewCompleteBehaviour : LearningCompleteBehaviourBase
    {
        protected override void InitExitButton()
        {
            exitButton.OnClickAsObservable()
                .Subscribe(static _ => WindowsController.Instance.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);
        }
    }
}