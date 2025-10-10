using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using VContainer.Unity;

namespace Source.Scripts.Main.Data.CurrentWord
{
    internal sealed class MainCurrentWordFactory :
        ResolverStateFactory<PracticeState, MainCurrentWordService, ICurrentWordService>,
        ICurrentWordFactory
    {
        internal MainCurrentWordFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}