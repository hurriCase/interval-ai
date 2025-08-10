using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
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
        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);

        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IAppConfig _appConfig;

        internal ReactiveCommand<ModuleType> SwitchModuleCommand { get; } = new();

        private WordEntry WordEntry => _wordsRepository.CurrentWordsByState.CurrentValue[_practiceState];

        private PracticeState _practiceState;

        internal void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            foreach (var module in _practiceModules)
                module.Init(this);

            _wordsRepository.CurrentWordsByState
                .Select(_practiceState, (currentWordsByState, state)
                    => currentWordsByState[state])
                .Subscribe(this, static (_, self) => self.HandleNewWord())
                .RegisterTo(destroyCancellationToken);

            SwitchModuleCommand
                .Subscribe(this, (moduleType, self) => self.SwitchModule(moduleType))
                .RegisterTo(destroyCancellationToken);

            HandleNewWord();
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