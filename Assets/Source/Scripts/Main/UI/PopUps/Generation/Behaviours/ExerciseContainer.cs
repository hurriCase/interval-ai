using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.Main.UI.PopUps.Generation.Behaviours.PracticeContainer;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours
{
    internal sealed class ExerciseContainer : MonoBehaviour
    {
        [SerializeField] private BrowsingContainer _browsingContainer;
        [SerializeField] private PracticeContainerBase _practicingContainer;

        internal void Init()
        {
            _browsingContainer.Init(this);
            _practicingContainer.Init();
        }

        internal void SwitchState(ExerciseState state)
        {
            _browsingContainer.SetActive(state == ExerciseState.Browsing);
            _practicingContainer.SetActive(state == ExerciseState.Practicing);
        }

        internal void UpdatePracticeContainer(ExerciseEntry exerciseEntry)
        {
            _practicingContainer.UpdateView(exerciseEntry);
        }
    }
}