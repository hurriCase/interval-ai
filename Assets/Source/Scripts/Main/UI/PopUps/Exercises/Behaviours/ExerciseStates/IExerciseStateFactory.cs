using Source.Scripts.Core.Repositories.Exercises;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates
{
    internal interface IExerciseStateFactory
    {
        IExerciseStateService GetOrCreate(ExerciseType practiceState);
    }
}