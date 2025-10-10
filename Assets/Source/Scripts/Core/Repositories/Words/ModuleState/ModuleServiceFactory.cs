using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using VContainer.Unity;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleServiceFactory :
        ResolverStateFactory<PracticeState, ModuleStateService, IModuleStateService>,
        IModuleStateFactory
    {
        internal ModuleServiceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}