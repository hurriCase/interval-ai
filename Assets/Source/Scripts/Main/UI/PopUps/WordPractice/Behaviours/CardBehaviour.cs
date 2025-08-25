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

        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [Inject] private ICurrentWordsService _currentWordsService;
        [Inject] private IAppConfig _appConfig;

        internal ReactiveCommand<ModuleType> SwitchModuleCommand { get; } = new();

        private WordEntry WordEntry => _currentWordsService.CurrentWordsByState.CurrentValue[_practiceState];

        private PracticeState _practiceState;

        internal void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            _wordProgressBehaviour.Init();

            _currentWordsService.CurrentWordsByState
                .Select(practiceState, (currentWordsByState, state) => currentWordsByState[state])
                .Where(currentWord => currentWord != null)
                .Subscribe(_wordProgressBehaviour,
                    static (currentWord, behaviour) => behaviour.UpdateProgress(currentWord))
                .RegisterTo(destroyCancellationToken);

            foreach (var module in _practiceModules)
                module.Init(this);

            _currentWordsService.CurrentWordsByState
                .Select(_practiceState, (currentWordsByState, state) => currentWordsByState[state])
                .SubscribeAndRegister(this, self => self.HandleNewWord());

            SwitchModuleCommand.SubscribeAndRegister(this, (moduleType, self) => self.SwitchModule(moduleType));

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