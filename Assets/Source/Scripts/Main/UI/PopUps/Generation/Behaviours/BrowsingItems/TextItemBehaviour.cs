using CustomUtils.Runtime.Extensions;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.BrowsingItems
{
    internal sealed class TextItemBehaviour : ExerciseItemBehaviourBase
    {
        protected override void OnUpdateView()
        {
            openExerciseButton.SetActive(contentText.IsTruncated);
        }
    }
}