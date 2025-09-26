using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.GenerationSettings.Behaviours
{
    internal sealed class LanguageToggle : ToggleComponent
    {
        private LanguageType _currentLanguageType;

        private IGenerationSettingsRepository _generationSettingsRepository;
        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationKeysDatabase _localizationKeysDatabase;

        [Inject]
        public void Inject(
            IGenerationSettingsRepository generationSettingsRepository,
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationKeysDatabase localizationKeysDatabase)
        {
            _generationSettingsRepository = generationSettingsRepository;
            _languageSettingsRepository = languageSettingsRepository;
            _localizationKeysDatabase = localizationKeysDatabase;
        }

        internal void Init(LanguageType learningType)
        {
            _currentLanguageType = learningType;

            isOn = _currentLanguageType == _generationSettingsRepository.TranslateFromLanguageType.Value;
            this.OnValueChangedAsObservable()
                .Where(isOn => isOn)
                .SubscribeUntilDestroy(this, static self => self.ChangeLanguageType());

            _languageSettingsRepository.LanguageByType
                .Select(this, (currentLanguages, self) => currentLanguages[self._currentLanguageType])
                .SubscribeUntilDestroy(this, static (language, self) => self.UpdateLanguageTypeTexts(language));
        }

        private void ChangeLanguageType()
        {
            _generationSettingsRepository.TranslateFromLanguageType.Value = _currentLanguageType;
        }

        private void UpdateLanguageTypeTexts(SystemLanguage currentLanguages)
        {
            Text.text = _localizationKeysDatabase.GetLanguageLocalization(currentLanguages);
        }
    }
}