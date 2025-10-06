using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Button;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.BrowsingItems
{
    internal class ExerciseItemBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected MaxLinesLimiter contentText;
        [SerializeField] protected ButtonComponent openExerciseButton;

        private ExerciseContainer _exerciseContainer;
        private ExerciseEntry _exerciseEntry;

        internal void Init(ExerciseContainer exerciseContainer, ExerciseEntry exercise)
        {
            _exerciseEntry = exercise;

            _exerciseContainer = exerciseContainer;

            openExerciseButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SwitchState());

            OnInit(exercise);
        }

        protected virtual void OnInit(ExerciseEntry exercise) { }

        private void SwitchState()
        {
            _exerciseContainer.SwitchState(ExerciseState.Practicing);
            _exerciseContainer.UpdatePracticeContainer(_exerciseEntry);
        }

        internal void UpdateView(ExerciseEntry exercise)
        {
            contentText.SetText(exercise.Content.Learning);

            OnUpdateView();
        }

        protected virtual void OnUpdateView() { }
    }
}