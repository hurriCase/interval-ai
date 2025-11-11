using Source.Scripts.Core.Repositories.Exercises.Exercise;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.PracticeContainer
{
    internal sealed class TextPracticeContainer : PracticeContainerBase
    {
        [SerializeField] private TextMeshProUGUI _text;

        internal override void UpdateView(ExerciseEntry exerciseEntry)
        {
            _text.text = exerciseEntry.Content.Learning;
        }
    }
}