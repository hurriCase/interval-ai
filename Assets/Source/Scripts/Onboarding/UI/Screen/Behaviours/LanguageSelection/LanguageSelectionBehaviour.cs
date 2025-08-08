using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
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
        [Inject] private IDefaultSettingsConfig _defaultSettingsConfig;
        [Inject] private ILocalizationDatabase _localizationDatabase;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

        private EnumArray<LanguageType, EnumArray<Language, LanguageSelectionItem>> _createdLanguageSelectionItems =
            new(new EnumArray<Language, LanguageSelectionItem>(EnumMode.SkipFirst), EnumMode.SkipFirst);
        private Subject<Unit> _continueSubject;

        internal override void Init()
        {
            _nativeLanguagesAccordion.AccordionComponent.Init();
            _learningComponentAccordion.AccordionComponent.Init();

            foreach (var (language, localization) in _localizationDatabase.Languages.AsTuples())
            {
                CreateLanguageItem(language, localization, LanguageType.Native);
                CreateLanguageItem(language, localization, LanguageType.Learning);
            }

            _settingsRepository.LanguageByType
                .Subscribe(this, static (languageByType, behaviour)
                    => behaviour.SetActiveLanguageToggle(languageByType))
                .RegisterTo(destroyCancellationToken);
        }

        private void SetActiveLanguageToggle(EnumArray<LanguageType, Language> languages)
        {
            foreach (var (languageType, language) in languages.AsTuples())
            {
                var targetSelectionItem = _createdLanguageSelectionItems[languageType][language];
                targetSelectionItem.CheckboxComponent.isOn = true;
            }
        }

        private void CreateLanguageItem(Language language, string localization, LanguageType languageType)
        {
            var accordionItem = languageType == LanguageType.Learning
                ? _learningComponentAccordion
                : _nativeLanguagesAccordion;

            var createdSpacing = _objectResolver.Instantiate(_spacingItem,
                accordionItem.AccordionComponent.HiddenContentContainer.RectTransform);

            accordionItem.AccordionComponent.HiddenContent.Value.Add(createdSpacing);

            CreateLanguageButton(accordionItem, language, localization, languageType);
        }

        private void CreateLanguageButton(LanguageAccordionItem accordionComponent, Language language, string localization,
            LanguageType languageType)
        {
            var createdLanguageItem = _objectResolver.Instantiate(_languageSelectionItem,
                accordionComponent.AccordionComponent.HiddenContentContainer.RectTransform);

            createdLanguageItem.LanguageText.text = localization;

            createdLanguageItem.CheckboxComponent.OnPointerClickAsObservable()
                .Subscribe((behaviour: this, language, languageType), static (_, tuple)
                    => tuple.behaviour._settingsRepository.SetLanguage(tuple.language, tuple.languageType))
                .RegisterTo(destroyCancellationToken);

            createdLanguageItem.CheckboxComponent.group = accordionComponent.ToggleGroup;

            _addressablesLoader.AssignImageAsync(createdLanguageItem.Icon,
                _defaultSettingsConfig.LanguageSprites[language], destroyCancellationToken);

            accordionComponent.AccordionComponent.HiddenContent.Value.Add(createdLanguageItem.AccordionItem);

            AssignLanguageItem(createdLanguageItem, language, languageType);
        }

        private void AssignLanguageItem(LanguageSelectionItem createdLanguageItem, Language language,
            LanguageType languageType)
        {
            var createdLanguageSelectionItem =
                _createdLanguageSelectionItems[languageType];

            createdLanguageSelectionItem[language] = createdLanguageItem;
            _createdLanguageSelectionItems[languageType] = createdLanguageSelectionItem;
        }
    }
}