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

        private WordEntry WordEntry => _currentWordsService.CurrentWordsByState.CurrentValue[_practiceState];

        private PracticeState _practiceState;

        private ICurrentWordsService _currentWordsService;
        private IModuleStateFactory _moduleStateFactory;

        [Inject]
        internal void Inject(ICurrentWordsService currentWordsService, IModuleStateFactory moduleStateFactory)
        {
            _currentWordsService = currentWordsService;
            _moduleStateFactory = moduleStateFactory;
        }

        internal void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            _wordProgressBehaviour.Init();

            _currentWordsService.CurrentWordsByState
                .Select(practiceState, (currentWordsByState, state) => currentWordsByState[state])
                .Where(currentWord => currentWord != null)
                .SubscribeUntilDestroy(_wordProgressBehaviour,
                    static (currentWord, wordProgress) => wordProgress.UpdateProgress(currentWord));

            foreach (var module in _practiceModules)
                module.Init(practiceState);

            var moduleStateService = _moduleStateFactory.GetOrCreate(practiceState);
            moduleStateService.CurrentState
                .Where(this, (currentState, self) => currentState != ModuleType.None && self.WordEntry != null)
                .SubscribeUntilDestroy(this, static (state, self) => self.SwitchModule(state));
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