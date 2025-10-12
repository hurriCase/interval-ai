using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    [Preserve]
    internal sealed class WordAdvanceFactory :
        ResolverStateFactory<PracticeState, WordAdvanceService, IWordAdvanceService>,
        IWordAdvanceFactory
    {
        [Preserve]
        internal WordAdvanceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}