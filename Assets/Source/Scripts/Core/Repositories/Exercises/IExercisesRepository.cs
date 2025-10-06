using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Repositories.Exercises.Exercise;

namespace Source.Scripts.Core.Repositories.Exercises
{
    internal interface IExercisesRepository
    {
        ReadOnlyReactiveProperty<EnumArray<ExerciseType, Dictionary<int, ExerciseEntry>>> Exercises { get; }
        void RemoveExercise(ExerciseType exerciseType, int exerciseId);
    }
}