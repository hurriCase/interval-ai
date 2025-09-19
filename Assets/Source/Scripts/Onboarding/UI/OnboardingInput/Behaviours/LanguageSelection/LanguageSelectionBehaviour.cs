using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private EnumArray<LanguageType, LanguageAccordionItem> _languageAccordionItems =
            new(EnumMode.SkipFirst);

        private IAppConfig _appConfig;

        [Inject]
        internal void Inject(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        internal override void Init()
        {
            foreach (var (languageType, languages) in _appConfig.SupportedLanguages.AsTuples())
                _languageAccordionItems[languageType].Init(languageType, languages);
        }
    }
}