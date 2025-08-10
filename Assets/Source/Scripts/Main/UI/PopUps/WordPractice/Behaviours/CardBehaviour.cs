using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class CardBehaviour : MonoBehaviour
    {
        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);

        [Inject] private IAppConfig _appConfig;

        internal ReactiveProperty<WordEntry> WordEntry { get; } = new();

        private PracticeState _practiceState;

        internal void Init(PracticeState practiceState)
        {
            _practiceState = practiceState;

            foreach (var module in _practiceModules)
                module.Init(this);

            WordEntry.Subscribe(this, (_, self) => self.UpdateView())
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateView()
        {
            var isWordAvailable = WordEntry.Value != null && WordEntry.Value.Cooldown <= DateTime.Now;

            gameObject.SetActive(isWordAvailable);

            if (isWordAvailable is false)
                return;

            SwitchModule(_appConfig.PracticeToModuleType[_practiceState]);
        }

        internal void SwitchModule(ModuleType moduleType)
        {
            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(WordEntry.Value);
                module.SetActive(type == moduleType);
            }
        }
    }
}