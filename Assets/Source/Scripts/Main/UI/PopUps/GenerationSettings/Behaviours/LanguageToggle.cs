using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.Selection.LocalizationData;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class LanguageToggle : StateToggle
    {
        private LanguageKeyLocalizationConfig _languageKeyLocalizationConfig;
        private LanguageType _currentLanguageType;

        private IGenerationSettingsRepository _generationSettingsRepository;
        private ILanguageSettingsRepository _languageSettingsRepository;

        [Inject]
        public void Inject(
            IGenerationSettingsRepository generationSettingsRepository,
            ILanguageSettingsRepository languageSettingsRepository)
        {
            _generationSettingsRepository = generationSettingsRepository;
            _languageSettingsRepository = languageSettingsRepository;
        }

        internal void Init(LanguageKeyLocalizationConfig languageKeyLocalizationConfig, LanguageType learningType)
        {
            _languageKeyLocalizationConfig = languageKeyLocalizationConfig;
            _currentLanguageType = learningType;

            isOn = _currentLanguageType == _generationSettingsRepository.TranslateFromLanguageType.Value;
            this.OnValueChangedAsObservable()
                .Where(static isOn => isOn)
                .SubscribeUntilDestroy(this, static self => self.ChangeLanguageType());

            _languageSettingsRepository.LanguageByType
                .Select(this, static (currentLanguages, self) => currentLanguages[self._currentLanguageType])
                .SubscribeUntilDestroy(this, static (language, self) => self.UpdateLanguageTypeTexts(language));
        }

        private void ChangeLanguageType()
        {
            _generationSettingsRepository.TranslateFromLanguageType.Value = _currentLanguageType;
        }

        private void UpdateLanguageTypeTexts(SystemLanguage currentLanguages)
        {
            Text.text = _languageKeyLocalizationConfig.GetLocalization(currentLanguages);
        }
    }
}