using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    [Preserve]
    internal sealed class ModuleServiceFactory :
        ResolverStateFactory<PracticeState, ModuleStateService, IModuleStateService>,
        IModuleStateFactory
    {
        [Preserve]
        internal ModuleServiceFactory(LifetimeScope parentScope) : base(parentScope) { }
    }
}