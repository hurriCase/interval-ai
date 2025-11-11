using Source.Scripts.Core.DI;
using Source.Scripts.Core.Repositories.Exercises;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.ExerciseStates
{
    [Preserve]
    internal sealed class ExerciseStateFactory :
        ResolverStateFactory<ExerciseType, ExerciseStateService, IExerciseStateService>,
        IExerciseStateFactory
    {
        [Preserve]
        internal ExerciseStateFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}