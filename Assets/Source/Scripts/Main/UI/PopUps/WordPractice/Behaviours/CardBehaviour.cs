using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class CardBehaviour : MonoBehaviour
    {
        [SerializeField] private EnumArray<ModuleType, PracticeModule> _practiceModules;

        [SerializeField] private GameObject _headerContainer;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        private WordEntry WordEntry => _currentWordService.CurrentWord.CurrentValue;

        private ICurrentWordService _currentWordService;

        private ICurrentWordFactory _currentWordFactory;
        private IModuleStateFactory _moduleStateFactory;

        [Inject]
        internal void Inject(ICurrentWordFactory currentWordFactory, IModuleStateFactory moduleStateFactory)
        {
            _currentWordFactory = currentWordFactory;
            _moduleStateFactory = moduleStateFactory;
        }

        internal void Init(PracticeState practiceState)
        {
            _wordProgressBehaviour.Init();

            _currentWordService = _currentWordFactory.GetOrCreate(practiceState);

            _currentWordService.CurrentWord
                .Where(static currentWord => currentWord != null)
                .SubscribeUntilDestroy(_wordProgressBehaviour,
                    static (currentWord, wordProgress) => wordProgress.UpdateProgress(currentWord));

            foreach (var module in _practiceModules)
                module.Init(practiceState);

            var moduleStateService = _moduleStateFactory.GetOrCreate(practiceState);
            moduleStateService.CurrentModule
                .Where(this, static (_, self) => self.WordEntry != null)
                .SubscribeUntilDestroy(this, static (state, self) => self.SwitchModule(state));
        }

        private void SwitchModule(ModuleType moduleType)
        {
            _headerContainer.SetActive(moduleType != ModuleType.FirstShow);

            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(WordEntry);
                module.SetActive(type == moduleType);
            }
        }
    }
}