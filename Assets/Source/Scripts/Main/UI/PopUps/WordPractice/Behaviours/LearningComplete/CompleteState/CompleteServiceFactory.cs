using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState
{
    [Preserve]
    internal sealed class CompleteServiceFactory :
        ResolverStateFactory<PracticeState, CompleteStateService, ICompleteStateService>,
        ICompleteServiceFactory
    {
        [Preserve]
        internal CompleteServiceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}