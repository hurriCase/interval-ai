using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Main.UI.PopUps.Generation.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Generation
{
    internal sealed class ExercisesPopUp : PopUpBase
    {
        [SerializeField]
        private EnumArray<ExerciseType, ExerciseContainer> _exercisesContainers = new(EnumMode.SkipFirst);

        [SerializeField] private TabsController<ExerciseType> _tabsController;

        internal override void Init()
        {
            _tabsController.Init(ExerciseType.Sentences, destroyCancellationToken);

            foreach (var (state, tab) in _tabsController.Tabs.AsTuples())
            {
                tab.SwitchToggle.OnPointerClickAsObservable()
                    .SubscribeUntilDestroy(this, state, static (state, self) => self.ResetExerciseState(state));
            }

            foreach (var exercisesContainerBehaviour in _exercisesContainers)
                exercisesContainerBehaviour.Init();
        }

        internal override UniTask ShowAsync()
        {
            foreach (var (state, _) in _exercisesContainers.AsTuples())
                ResetExerciseState(state);

            return base.ShowAsync();
        }

        private void ResetExerciseState(ExerciseType exerciseType)
        {
            _exercisesContainers[exerciseType].SwitchState(ExerciseState.Browsing);
        }
    }
}