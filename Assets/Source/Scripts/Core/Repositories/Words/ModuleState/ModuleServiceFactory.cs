using Source.Scripts.Core.Configs;
using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleServiceFactory : StateFactoryBase<PracticeState, IModuleStateService>,
        IModuleStateFactory
    {
        private readonly ICurrentWordFactory _currentWordFactory;
        private readonly IAppConfig _appConfig;

        internal ModuleServiceFactory(ICurrentWordFactory currentWordFactory, IAppConfig appConfig)
        {
            _currentWordFactory = currentWordFactory;
            _appConfig = appConfig;
        }

        protected override IModuleStateService CreateService(PracticeState practiceState) =>
            new ModuleStateService(_currentWordFactory, _appConfig, practiceState);
    }
}