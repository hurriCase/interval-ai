using System;
using System.Collections.Generic;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleStateServiceFactory : IModuleStateServiceFactory, IDisposable
    {
        private readonly Dictionary<PracticeState, IModuleStateService> _cachedServices = new();
        private readonly ICurrentWordsService _currentWordsService;
        private readonly IAppConfig _appConfig;

        internal ModuleStateServiceFactory(ICurrentWordsService currentWordsService, IAppConfig appConfig)
        {
            _currentWordsService = currentWordsService;
            _appConfig = appConfig;
        }

        public IModuleStateService GetOrCreate(PracticeState practiceState)
        {
            if (_cachedServices.TryGetValue(practiceState, out var service))
                return service;

            service = new ModuleStateService(_currentWordsService, _appConfig, practiceState);
            _cachedServices[practiceState] = service;
            return service;
        }

        public void Dispose()
        {
            foreach (var service in _cachedServices.Values)
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }

            _cachedServices.Clear();
        }
    }
}