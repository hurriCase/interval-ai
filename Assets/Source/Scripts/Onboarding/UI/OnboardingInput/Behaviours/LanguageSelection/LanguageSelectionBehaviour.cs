using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private LanguageAccordionItem _nativeLanguagesAccordion;
        [SerializeField] private LanguageAccordionItem _learningComponentAccordion;
        [SerializeField] private LanguageSelectionItem _languageSelectionItem;
        [SerializeField] private AccordionItem _spacingItem;

        [Inject] private ILanguageSettingsRepository _languageSettingsRepository;
        [Inject] private IAppConfig _appConfig;
        [Inject] private ISpriteReferences _spriteReferences;
        [Inject] private ILocalizationDatabase _localizationDatabase;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

        private EnumArray<LanguageType, EnumArray<SystemLanguage, LanguageSelectionItem>>
            _createdLanguageSelectionItems = new(() =>
                new EnumArray<SystemLanguage, LanguageSelectionItem>(EnumMode.Default), EnumMode.SkipFirst);

        private Subject<Unit> _continueSubject;

        internal override void Init()
        {
            _nativeLanguagesAccordion.AccordionComponent.Init();
            _learningComponentAccordion.AccordionComponent.Init();

            foreach (var (languageType, systemLanguages) in
                     _appConfig.SupportedLanguages.AsTuples())
            {
                foreach (var systemLanguage in systemLanguages)
                    CreateLanguageItem(languageType, systemLanguage, _localizationDatabase.Languages[systemLanguage]);
            }

            _languageSettingsRepository.LanguageByType.SubscribeAndRegister(this,
                static (languageByType, self) => self.SetActiveLanguageToggle(languageByType));
        }

        private void SetActiveLanguageToggle(EnumArray<LanguageType, SystemLanguage> languages)
        {
            foreach (var (languageType, language) in languages.AsTuples())
            {
                var targetSelectionItem = _createdLanguageSelectionItems[languageType][language];
                targetSelectionItem.CheckboxComponent.isOn = true;
            }
        }

        private void CreateLanguageItem(LanguageType languageType, SystemLanguage language, string localization)
        {
            var accordionItem = languageType == LanguageType.Learning
                ? _learningComponentAccordion
                : _nativeLanguagesAccordion;

            var createdSpacing = _objectResolver.Instantiate(_spacingItem,
                accordionItem.AccordionComponent.HiddenContentContainer.RectTransform);

            accordionItem.AccordionComponent.HiddenContent.Add(createdSpacing);

            CreateLanguageButton(languageType, language, accordionItem, localization);
        }

        private void CreateLanguageButton(
            LanguageType languageType,
            SystemLanguage language,
            LanguageAccordionItem accordionComponent,
            string localization)
        {
            var createdLanguageItem = _objectResolver.Instantiate(_languageSelectionItem,
                accordionComponent.AccordionComponent.HiddenContentContainer.RectTransform);

            createdLanguageItem.gameObject.name = ZString.Format("{0}, {1}", languageType, language);

            // ReSharper disable once HeapView.BoxingAllocation . It's find, because this is done for safety reason
            createdLanguageItem.LanguageText.text = string.IsNullOrWhiteSpace(localization)
                ? language.ToString()
                : localization;

            createdLanguageItem.CheckboxComponent.OnPointerClickAsObservable()
                .SubscribeAndRegister(this, (language, languageType), static (tuple, self)
                    => self._languageSettingsRepository.SetLanguage(tuple.language, tuple.languageType));

            createdLanguageItem.CheckboxComponent.group = accordionComponent.ToggleGroup;

            _addressablesLoader.AssignImageAsync(createdLanguageItem.Icon,
                _spriteReferences.LanguageSprites[language], destroyCancellationToken);

            accordionComponent.AccordionComponent.HiddenContent.Add(createdLanguageItem.AccordionItem);

            AssignLanguageItem(languageType, language, createdLanguageItem);
        }

        private void AssignLanguageItem(
            LanguageType languageType,
            SystemLanguage language,
            LanguageSelectionItem createdLanguageItem)
        {
            var createdLanguageSelectionItem =
                _createdLanguageSelectionItems[languageType];

            createdLanguageSelectionItem[language] = createdLanguageItem;
            _createdLanguageSelectionItems[languageType] = createdLanguageSelectionItem;
        }
    }
}