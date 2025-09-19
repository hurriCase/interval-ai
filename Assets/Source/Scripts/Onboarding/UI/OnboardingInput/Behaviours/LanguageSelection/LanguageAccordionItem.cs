using System.Collections.Generic;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageAccordionItem : MonoBehaviour
    {
        [SerializeField] private AccordionComponent _accordionComponent;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private ToggleComponent _languageSelectionItem;

        private readonly Dictionary<SystemLanguage, ToggleComponent> _createdLanguageItems = new();

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

        internal void Init(LanguageType languageType, SystemLanguage[] languages)
        {
            _currentLanguageType = languageType;

            CreateLanguageSelectionItems(languages);

            _languageSettingsRepository.LanguageByType
                .Select(this, (languageByType, self) => languageByType[self._currentLanguageType])
                .SubscribeAndRegister(this,
                    static (language, self) => self._createdLanguageItems[language].isOn = true);
        }

        private void CreateLanguageSelectionItems(SystemLanguage[] languages)
        {
            foreach (var language in languages)
            {
                var container = _accordionComponent.HiddenContentContainer;
                var createdLanguageItem = Instantiate(_languageSelectionItem, container);

                createdLanguageItem.Text.text = _localizationDatabase.GetLanguageName(language);
                createdLanguageItem.group = _toggleGroup;
                createdLanguageItem.OnPointerClickAsObservable()
                    .SubscribeAndRegister(this, language, static (language, self) => self.SetLanguage(language));

                var sprite = _spriteReferences.LanguageSprites[language];
                _addressablesLoader.AssignImageAsync(createdLanguageItem.Image, sprite, destroyCancellationToken);

                _createdLanguageItems[language] = createdLanguageItem;
            }
        }

        private void SetLanguage(SystemLanguage systemLanguage)
        {
            _languageSettingsRepository.SetLanguage(systemLanguage, _currentLanguageType);
        }
    }
}