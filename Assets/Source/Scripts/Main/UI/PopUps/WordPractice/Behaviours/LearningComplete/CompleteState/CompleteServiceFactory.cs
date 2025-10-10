using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState
{
    internal sealed class CompleteServiceFactory :
        ResolverStateFactory<PracticeState, CompleteStateService, ICompleteStateService>,
        ICompleteServiceFactory
    {
        internal CompleteServiceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}