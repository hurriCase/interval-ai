using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class CardBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _cardContainer;

        [SerializeField] private ModuleType _initialModuleType;
        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);

        internal ReactiveProperty<WordEntry> WordEntry { get; } = new();

        internal void Init()
        {
            foreach (var module in _practiceModules)
                module.Init(this);

            WordEntry.Subscribe(this, (_, self) => self.UpdateView())
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateView()
        {
            var isWordAvailable = WordEntry.Value != null && WordEntry.Value.Cooldown <= DateTime.Now;

            _cardContainer.SetActive(isWordAvailable);

            if (isWordAvailable is false)
                return;

            SwitchModule(_initialModuleType);
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