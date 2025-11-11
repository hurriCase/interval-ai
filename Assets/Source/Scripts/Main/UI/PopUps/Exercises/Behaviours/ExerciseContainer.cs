using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.PracticeContainer;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours
{
    internal sealed class ExerciseContainer : MonoBehaviour
    {
        [SerializeField] private BrowsingContainer _browsingContainer;
        [SerializeField] private PracticeContainerBase _practicingContainer;

        private IExerciseStateFactory _exerciseStateFactory;
        private IExerciseStateService _exerciseStateService;

        [Inject]
        internal void Inject(IExerciseStateFactory exerciseStateFactory)
        {
            _exerciseStateFactory = exerciseStateFactory;
        }

        internal void Init(ExerciseType exerciseType)
        {
            _browsingContainer.Init();
            _practicingContainer.Init();

            _exerciseStateService = _exerciseStateFactory.GetOrCreate(exerciseType);
            _exerciseStateService.CurrentState
                .SubscribeUntilDestroy(this, static (exerciseState, self) => self.SwitchState(exerciseState));

            _exerciseStateService.CurrentEntry
                .Where(static exerciseEntry => exerciseEntry != null)
                .SubscribeUntilDestroy(this,
                    static (exerciseEntry, self) => self.UpdatePracticeContainer(exerciseEntry));
        }

        private void SwitchState(ExerciseState state)
        {
            _browsingContainer.SetActive(state == ExerciseState.Browsing);
            _practicingContainer.SetActive(state == ExerciseState.Practicing);
        }

        private void UpdatePracticeContainer(ExerciseEntry exerciseEntry)
        {
            _practicingContainer.UpdateView(exerciseEntry);
        }
    }
}