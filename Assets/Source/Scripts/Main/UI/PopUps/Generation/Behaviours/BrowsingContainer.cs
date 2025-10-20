using System.Collections.Generic;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.Main.UI.PopUps.Generation.Behaviours.BrowsingItems;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours
{
    internal sealed class BrowsingContainer : MonoBehaviour
    {
        [SerializeField] private ExerciseType _exerciseType;
        [SerializeField] private RectTransform _container;
        [SerializeField] private ExerciseItemBehaviourBase _itemPrefab;

        private UIPoolWithData<ExerciseEntry, ExerciseItemBehaviourBase> _generationsPoolWithData;

        private IExercisesRepository _exercisesRepository;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IExercisesRepository exercisesRepository, IObjectResolver objectResolver)
        {
            _exercisesRepository = exercisesRepository;
            _objectResolver = objectResolver;
        }

        internal void Init(ExerciseContainer exercisesPopUp)
        {
            var uiPoolEvents = new UIPoolEvents<ExerciseEntry, ExerciseItemBehaviourBase>(
                (exercise, prefab) =>
                {
                    prefab.Init(exercisesPopUp, exercise);
                    prefab.UpdateView(exercise);
                },
                static (exercise, item) => item.UpdateView(exercise));

            _generationsPoolWithData = new UIPoolWithData<ExerciseEntry, ExerciseItemBehaviourBase>(
                _itemPrefab,
                _container,
                uiPoolEvents,
                _objectResolver);

            _exercisesRepository.Exercises
                .Select(this, static (exercises, self) => exercises[self._exerciseType])
                .SubscribeUntilDestroy(this,
                    static (exercises, self) => self._generationsPoolWithData.EnsureCount(exercises.Values));
        }
    }
}