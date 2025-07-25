using R3;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data;
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

        [Inject] private IUserRepository _userRepository;
        [Inject] private ILocalizationDatabase _localizationDatabase;

        internal override void Init()
        {
            foreach (var (language, localization) in _localizationDatabase.Languages.AsTuples())
            {
                CreateLanguageItem(language, localization, LanguageType.Native);
                CreateLanguageItem(language, localization, LanguageType.Learning);
            }

            _nativeLanguagesAccordion.SwitchContent(false);
            _learningComponentAccordion.SwitchContent(false);
        }

        private void CreateLanguageItem(Language language, string localization, LanguageType languageType)
        {
            var container = languageType == LanguageType.Learning
                ? _learningComponentAccordion
                : _nativeLanguagesAccordion;

            var createdLanguageItem = Instantiate(_languageItem, transform);

            createdLanguageItem.Text.text = localization;

            createdLanguageItem.Button.OnClickAsObservable()
                .Subscribe((type: language, _userRepository, languageType), static (_, tuple)
                    =>
                {
                    var enumArray = tuple._userRepository.LanguageByType.Value;
                    enumArray[tuple.languageType] = tuple.type;
                    tuple._userRepository.LanguageByType.Value = enumArray;
                })
                .RegisterTo(destroyCancellationToken);

            container.HiddenContent.Add(createdLanguageItem.gameObject);

            var createdSpacing = Instantiate(_spacing, transform);
            createdSpacing.aspectRatio = _spacingRatio;

            container.HiddenContent.Add(createdSpacing.gameObject);
        }
    }
}