using R3;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using UnityEngine.Scripting;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates
{
    [Preserve]
    internal sealed class ExerciseStateService : IExerciseStateService
    {
        public ReadOnlyReactiveProperty<ExerciseEntry> CurrentEntry => _currentEntry;
        public ReadOnlyReactiveProperty<ExerciseState> CurrentState => _currentState;

        private readonly ReactiveProperty<ExerciseEntry> _currentEntry = new();
        private readonly ReactiveProperty<ExerciseState> _currentState = new(ExerciseState.Browsing);

        public void SetState(ExerciseState state, ExerciseEntry currentEntry = null)
        {
            _currentState.Value = state;
            _currentEntry.Value = currentEntry;
        }
    }
}