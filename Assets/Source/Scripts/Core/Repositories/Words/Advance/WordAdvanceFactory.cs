using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using VContainer.Unity;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal sealed class WordAdvanceFactory :
        ResolverStateFactory<PracticeState, WordAdvanceService, IWordAdvanceService>,
        IWordAdvanceFactory
    {
        internal WordAdvanceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}