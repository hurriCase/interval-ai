using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Exercises
{
    internal sealed class ExercisesPopUp : PopUpBase
    {
        [SerializeField] private EnumArray<ExerciseType, ExerciseContainer> _exercisesContainers;
        [SerializeField] private TabsController<ExerciseType> _tabsController;

        private readonly EnumArray<ExerciseType, IExerciseStateService> _sentenceStateService = new();

        private IExerciseStateFactory _exerciseStateFactory;

        [Inject]
        internal void Inject(IExerciseStateFactory exerciseStateFactory)
        {
            _exerciseStateFactory = exerciseStateFactory;
        }

        internal override void Init()
        {
            foreach (var valueTuple in _sentenceStateService.AsTuples())
                _sentenceStateService[valueTuple.Key] = _exerciseStateFactory.GetOrCreate(valueTuple.Key);

            _tabsController.Init(ExerciseType.Sentences, destroyCancellationToken);

            foreach (var (state, tab) in _tabsController.Tabs.AsTuples())
            {
                tab.SwitchToggle.OnPointerClickAsObservable()
                    .SubscribeUntilDestroy(this, state, static (state, self) => self.ResetExerciseState(state));
            }

            foreach (var (exerciseType, container) in _exercisesContainers.AsTuples())
                container.Init(exerciseType);
        }

        internal override UniTask ShowAsync()
        {
            foreach (var (state, _) in _exercisesContainers.AsTuples())
                ResetExerciseState(state);

            return base.ShowAsync();
        }

        private void ResetExerciseState(ExerciseType exerciseType)
        {
            _sentenceStateService[exerciseType].SetState(ExerciseState.Browsing);
        }
    }
}