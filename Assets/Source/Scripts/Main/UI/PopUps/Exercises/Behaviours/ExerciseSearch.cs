using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.Main.UI.Shared;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours
{
    internal sealed class ExerciseSearch : SearchBase<ExerciseEntry>
    {
        [SerializeField] private ExerciseType _exerciseType;

        protected override Dictionary<int, ExerciseEntry> SearchResults
            => _exercisesRepository.Exercises.CurrentValue[_exerciseType];

        private IExercisesRepository _exercisesRepository;

        [Inject]
        internal void Inject(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository;
        }
    }
}