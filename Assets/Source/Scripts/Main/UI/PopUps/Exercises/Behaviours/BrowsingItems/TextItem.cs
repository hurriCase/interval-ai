using CustomUtils.Runtime.Extensions;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.BrowsingItems
{
    internal sealed class TextItem : ExerciseItemBase
    {
        protected override void OnUpdateView()
        {
            openExerciseButton.SetActive(contentText.IsTruncated);
        }
    }
}