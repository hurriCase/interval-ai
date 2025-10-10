using System;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal sealed class ModuleStateService : IModuleStateService, IDisposable
    {
        public ReadOnlyReactiveProperty<ModuleType> CurrentState => _currentState;
        private readonly ReactiveProperty<ModuleType> _currentState;

        private readonly IDisposable _disposable;

        private readonly IAppConfig _appConfig;
        private readonly PracticeState _practiceState;

        internal ModuleStateService(
            ICurrentWordFactory currentWordFactory,
            IAppConfig appConfig,
            PracticeState practiceState)
        {
            _appConfig = appConfig;
            _practiceState = practiceState;

            _currentState = new ReactiveProperty<ModuleType>(appConfig.PracticeToModuleType[practiceState]);

            var currentWordService = currentWordFactory.GetOrCreate(practiceState);
            _disposable = currentWordService.CurrentWord
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