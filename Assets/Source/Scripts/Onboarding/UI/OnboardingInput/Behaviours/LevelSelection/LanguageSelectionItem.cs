using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class LanguageSelectionItem : MonoBehaviour
    {
        [SerializeField] private StateToggle _stateToggle;
        [SerializeField] private Image _icon;

        private LanguageType _currentLanguageType;

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationDatabase _localizationDatabase;
        private IAddressablesLoader _addressablesLoader;
        private ISpriteReferences _spriteReferences;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationDatabase localizationDatabase,
            IAddressablesLoader addressablesLoader,
            ISpriteReferences spriteReferences)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _localizationDatabase = localizationDatabase;
            _addressablesLoader = addressablesLoader;
            _spriteReferences = spriteReferences;
        }

        internal void Init(ToggleGroup toggleGroup, SystemLanguage targetLanguage, LanguageType languageType)
        {
            _currentLanguageType = languageType;

            _languageSettingsRepository.LanguageByType
                .Select(this, static (languageByType, self) => languageByType[self._currentLanguageType])
                .Where(targetLanguage, static (language, targetLanguage) => language == targetLanguage)
                .SubscribeUntilDestroy(this, static self => self._stateToggle.isOn = true);

            _stateToggle.Text.text = _localizationDatabase.GetLanguageName(targetLanguage);
            _stateToggle.group = toggleGroup;
            _stateToggle.OnPointerClickAsObservable()
                .SubscribeUntilDestroy(this, targetLanguage, static (language, self) => self.SetLanguage(language));

            var sprite = _spriteReferences.LanguageSprites[targetLanguage];
            _addressablesLoader.AssignImageAsync(_icon, sprite, destroyCancellationToken);
        }

        private void SetLanguage(SystemLanguage systemLanguage)
        {
            _languageSettingsRepository.SetLanguage(systemLanguage, _currentLanguageType);
        }
    }
}