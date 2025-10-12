using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Source.Scripts.Main.Data.CurrentWord
{
    [Preserve]
    internal sealed class CurrentWordFactory :
        ResolverStateFactory<PracticeState, CurrentWordService, ICurrentWordService>,
        ICurrentWordFactory
    {
        [Preserve]
        internal CurrentWordFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}