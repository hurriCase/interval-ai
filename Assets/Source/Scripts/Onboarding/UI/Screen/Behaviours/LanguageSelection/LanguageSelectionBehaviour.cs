using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Sprites;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private LanguageAccordionItem _nativeLanguagesAccordion;
        [SerializeField] private LanguageAccordionItem _learningComponentAccordion;
        [SerializeField] private LanguageSelectionItem _languageSelectionItem;
        [SerializeField] private AccordionItem _spacingItem;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private IAppConfig _appConfig;
        [Inject] private ISpriteReferences _spriteReferences;
        [Inject] private ILocalizationDatabase _localizationDatabase;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

        private EnumArray<LanguageType, EnumArray<SystemLanguage, LanguageSelectionItem>>
            _createdLanguageSelectionItems =
                new(() => new EnumArray<SystemLanguage, LanguageSelectionItem>(EnumMode.Default), EnumMode.SkipFirst);

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

            _settingsRepository.LanguageByType
                .Subscribe(this, static (languageByType, behaviour)
                    => behaviour.SetActiveLanguageToggle(languageByType))
                .RegisterTo(destroyCancellationToken);
        }

        private void SetActiveLanguageToggle(EnumArray<LanguageType, ReactiveProperty<SystemLanguage>> languages)
        {
            foreach (var (languageType, language) in languages.AsTuples())
            {
                var targetSelectionItem = _createdLanguageSelectionItems[languageType][language.Value];
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

            createdLanguageItem.gameObject.name = $"{languageType}, {language}";

            createdLanguageItem.LanguageText.text = string.IsNullOrWhiteSpace(localization)
                ? language.ToString()
                : localization;

            createdLanguageItem.CheckboxComponent.OnPointerClickAsObservable()
                .Subscribe((self: this, language, languageType), static (_, tuple)
                    => tuple.self._settingsRepository.SetLanguage(tuple.language, tuple.languageType))
                .RegisterTo(destroyCancellationToken);

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