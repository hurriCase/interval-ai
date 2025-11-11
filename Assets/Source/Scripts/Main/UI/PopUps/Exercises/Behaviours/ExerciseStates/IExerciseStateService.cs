using R3;
using Source.Scripts.Core.Repositories.Exercises.Exercise;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates
{
    internal interface IExerciseStateService
    {
        ReadOnlyReactiveProperty<ExerciseEntry> CurrentEntry { get; }
        ReadOnlyReactiveProperty<ExerciseState> CurrentState { get; }
        void SetState(ExerciseState state, ExerciseEntry currentEntry = null);
    }
}