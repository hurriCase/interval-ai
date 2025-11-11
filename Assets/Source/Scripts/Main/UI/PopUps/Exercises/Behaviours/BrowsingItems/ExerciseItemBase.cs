using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.BrowsingItems
{
    internal class ExerciseItemBase : View<ExerciseEntry>
    {
        [SerializeField] private ExerciseType _exerciseType;

        [SerializeField] protected MaxLinesLimiter contentText;
        [SerializeField] protected ThemeButton openExerciseButton;

        [Inject] private IExerciseStateFactory _exerciseStateFactory;

        private IExerciseStateService _exerciseStateService;
        private ExerciseEntry _exerciseEntry;

        internal override void Init(ExerciseEntry exercise)
        {
            _exerciseEntry = exercise;
            _exerciseStateService = _exerciseStateFactory.GetOrCreate(_exerciseType);

            openExerciseButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SwitchState());

            OnInit(exercise);
        }

        protected virtual void OnInit(ExerciseEntry exercise) { }

        private void SwitchState()
        {
            _exerciseStateService.SetState(ExerciseState.Practicing, _exerciseEntry);
        }

        internal override void UpdateView(ExerciseEntry exercise)
        {
            contentText.SetText(exercise.Content.Learning);

            OnUpdateView();
        }

        protected virtual void OnUpdateView() { }
    }
}