using Source.Scripts.Core.Configs;
using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleServiceFactory : StateFactoryBase<PracticeState, IModuleStateService>,
        IModuleStateFactory
    {
        private readonly ICurrentWordsService _currentWordsService;
        private readonly IAppConfig _appConfig;

        internal ModuleServiceFactory(ICurrentWordsService currentWordsService, IAppConfig appConfig)
        {
            _currentWordsService = currentWordsService;
            _appConfig = appConfig;
        }

        protected override IModuleStateService CreateService(PracticeState practiceState) =>
            new ModuleStateService(_currentWordsService, _appConfig, practiceState);
    }
}