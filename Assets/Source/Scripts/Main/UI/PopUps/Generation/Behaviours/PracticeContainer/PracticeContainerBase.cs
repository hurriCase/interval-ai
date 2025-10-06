using Source.Scripts.Core.Repositories.Exercises.Exercise;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.PracticeContainer
{
    internal abstract class PracticeContainerBase : MonoBehaviour
    {
        internal virtual void Init() { }
        internal abstract void UpdateView(ExerciseEntry exerciseEntry);
    }
}