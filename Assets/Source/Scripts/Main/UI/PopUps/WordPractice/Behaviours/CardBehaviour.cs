using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class CardBehaviour : MonoBehaviour
    {
        [SerializeField] private EnumArray<ModuleType, PracticeModule> _practiceModules
            = new(EnumMode.SkipFirst);
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        internal ReactiveCommand<ModuleType> SwitchModuleCommand { get; } = new();

        private WordEntry WordEntry => _currentWordsService.CurrentWordsByState.CurrentValue[_practiceState];

        private PracticeState _practiceState;

        private ICurrentWordsService _currentWordsService;
        private IAppConfig _appConfig;

        [Inject]
        internal void Inject(ICurrentWordsService currentWordsService, IAppConfig appConfig)
        {
            _currentWordsService = currentWordsService;
            _appConfig = appConfig;
        }

        internal void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            _wordProgressBehaviour.Init();

            _currentWordsService.CurrentWordsByState
                .Select(_practiceState, (currentWordsByState, state) => currentWordsByState[state])
                .Where(currentWord => currentWord != null)
                .Subscribe(_wordProgressBehaviour,
                    static (currentWord, wordProgress) => wordProgress.UpdateProgress(currentWord))
                .RegisterTo(destroyCancellationToken);

            foreach (var module in _practiceModules)
                module.Init(this);

            _currentWordsService.CurrentWordsByState
                .Select(_practiceState, (currentWordsByState, state) => currentWordsByState[state])
                .SubscribeUntilDestroy(this, self => self.HandleNewWord());

            SwitchModuleCommand.SubscribeUntilDestroy(this, (moduleType, self) => self.SwitchModule(moduleType));
        }

        private void HandleNewWord()
        {
            SwitchModuleCommand.ChangeCanExecute(WordEntry != null && WordEntry.Cooldown <= DateTime.Now);

            if (SwitchModuleCommand.CanExecute() is false)
                return;

            SwitchModule(_appConfig.PracticeToModuleType[_practiceState]);
        }

        private void SwitchModule(ModuleType moduleType)
        {
            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(WordEntry);
                module.SetActive(type == moduleType);
            }
        }
    }
}