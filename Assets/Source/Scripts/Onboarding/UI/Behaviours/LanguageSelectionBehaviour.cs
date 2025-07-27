using R3;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.UI.Behaviours
{
    internal sealed class LanguageSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private AccordionComponent _nativeLanguagesAccordion;
        [SerializeField] private AccordionComponent _learningComponentAccordion;
        [SerializeField] private ButtonTextComponent _languageItem;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _spacingRatio;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationDatabase _localizationDatabase;

        internal override void Init()
        {
            foreach (var (language, localization) in _localizationDatabase.Languages.AsTuples())
            {
                CreateLanguageItem(language, localization, LanguageType.Native);
                CreateLanguageItem(language, localization, LanguageType.Learning);
            }
        }

        private void CreateLanguageItem(Language language, string localization, LanguageType languageType)
        {
            var container = languageType == LanguageType.Learning
                ? _learningComponentAccordion
                : _nativeLanguagesAccordion;

            var createdLanguageItem = Instantiate(_languageItem, transform);

            createdLanguageItem.Text.text = localization;

            createdLanguageItem.Button.OnClickAsObservable()
                .Subscribe((type: language, _settingsRepository, languageType), static (_, tuple)
                    =>
                {
                    var enumArray = tuple._settingsRepository.LanguageByType.Value;
                    enumArray[tuple.languageType] = tuple.type;
                    tuple._settingsRepository.LanguageByType.Value = enumArray;
                })
                .RegisterTo(destroyCancellationToken);

            //container.HiddenContent.Add(createdLanguageItem.gameObject);

            var createdSpacing = Instantiate(_spacing, transform);
            createdSpacing.aspectRatio = _spacingRatio;

            //container.HiddenContent.Add(createdSpacing.gameObject);
        }
    }
}