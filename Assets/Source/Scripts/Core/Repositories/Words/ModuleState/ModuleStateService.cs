using System;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleStateService : IModuleStateService, IDisposable
    {
        public ReadOnlyReactiveProperty<ModuleType> CurrentState => _currentState;
        private readonly ReactiveProperty<ModuleType> _currentState;

        private readonly IAppConfig _appConfig;
        private readonly PracticeState _practiceState;
        private readonly IDisposable _disposable;

        internal ModuleStateService(
            ICurrentWordsService currentWordsService,
            IAppConfig appConfig,
            PracticeState practiceState)
        {
            _appConfig = appConfig;
            _practiceState = practiceState;

            _currentState = new ReactiveProperty<ModuleType>(appConfig.PracticeToModuleType[practiceState]);

            _disposable = currentWordsService.CurrentWordsByState
                .Select(practiceState, (currentWords, state) => currentWords[state])
                .Subscribe(this, (_, self) => self.HandleNewWord());
        }

        public void SetState(ModuleType moduleType)
        {
            _currentState.Value = moduleType;
        }

        private void HandleNewWord()
        {
            _currentState.OnNext(_appConfig.PracticeToModuleType[_practiceState]);
        }

        public void Dispose()
        {
            _currentState.Dispose();
            _disposable.Dispose();
        }
    }
}