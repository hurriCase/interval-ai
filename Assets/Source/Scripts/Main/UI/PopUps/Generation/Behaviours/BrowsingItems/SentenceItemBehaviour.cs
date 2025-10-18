using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Exercises;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.BrowsingItems
{
    internal sealed class SentenceItemBehaviour : ExerciseItemBehaviourBase
    {
        [SerializeField] private ThemeButton _deleteButton;

        private ExerciseEntry _currentSentence;

        private IExercisesRepository _exercisesRepository;

        [Inject]
        internal void Inject(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository;
        }

        protected override void OnInit(ExerciseEntry sentence)
        {
            _currentSentence = sentence;

            _deleteButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.RemoveSentence());
        }

        private void RemoveSentence()
        {
            _exercisesRepository.RemoveExercise(ExerciseType.Sentences, _currentSentence.Id);
        }
    }
}