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
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private LanguageAccordionItem _nativeLanguagesAccordion;
        [SerializeField] private LanguageAccordionItem _learningComponentAccordion;
        [SerializeField] private ToggleComponent _languageSelectionItem;

        private EnumArray<LanguageType, EnumArray<SystemLanguage, ToggleComponent>>
            _createdLanguageSelectionItems = new(() =>
                new EnumArray<SystemLanguage, ToggleComponent>(EnumMode.Default), EnumMode.SkipFirst);

        private Subject<Unit> _continueSubject;

        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationDatabase _localizationDatabase;
        private IAddressablesLoader _addressablesLoader;
        private ISpriteReferences _spriteReferences;
        private IObjectResolver _objectResolver;
        private IAppConfig _appConfig;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationDatabase localizationDatabase,
            IAddressablesLoader addressablesLoader,
            ISpriteReferences spriteReferences,
            IObjectResolver objectResolver,
            IAppConfig appConfig)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _localizationDatabase = localizationDatabase;
            _addressablesLoader = addressablesLoader;
            _spriteReferences = spriteReferences;
            _objectResolver = objectResolver;
            _appConfig = appConfig;
        }

        internal override void Init()
        {
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
                targetSelectionItem.isOn = true;
            }
        }

        private void CreateLanguageItem(LanguageType languageType, SystemLanguage language, string localization)
        {
            var accordionItem = languageType == LanguageType.Learning
                ? _learningComponentAccordion
                : _nativeLanguagesAccordion;

            CreateLanguageButton(languageType, language, accordionItem, localization);
        }

        private void CreateLanguageButton(
            LanguageType languageType,
            SystemLanguage language,
            LanguageAccordionItem accordionComponent,
            string localization)
        {
            var createdLanguageItem = _objectResolver.Instantiate(_languageSelectionItem,
                accordionComponent.AccordionComponent.HiddenContentContainer);

            createdLanguageItem.gameObject.name = ZString.Format("{0}, {1}", languageType, language);

            // ReSharper disable once HeapView.BoxingAllocation . It's fine, because this is done for safety reason
            createdLanguageItem.Text.text = localization.IsValid() is false
                ? language.ToString()
                : localization;

            createdLanguageItem.group = accordionComponent.ToggleGroup;
            createdLanguageItem.OnPointerClickAsObservable()
                .SubscribeAndRegister(this, (language, languageType), static (tuple, self)
                    => self._languageSettingsRepository.SetLanguage(tuple.language, tuple.languageType));

            _addressablesLoader.AssignImageAsync(createdLanguageItem.Image,
                _spriteReferences.LanguageSprites[language], destroyCancellationToken);

            AssignLanguageItem(languageType, language, createdLanguageItem);
        }

        private void AssignLanguageItem(
            LanguageType languageType,
            SystemLanguage language,
            ToggleComponent createdLanguageItem)
        {
            var createdLanguageSelectionItem =
                _createdLanguageSelectionItems[languageType];

            createdLanguageSelectionItem[language] = createdLanguageItem;
            _createdLanguageSelectionItems[languageType] = createdLanguageSelectionItem;
        }
    }
}