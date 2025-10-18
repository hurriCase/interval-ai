using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageAccordionItem : MonoBehaviour
    {
        [SerializeField] private AccordionComponent _accordionComponent;
        [SerializeField] private LanguageSelectionItem _languageSelectionItem;
        [SerializeField] private ToggleGroup _toggleGroup;

        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        internal void Init(LanguageType languageType, SystemLanguage[] languages)
        {
            foreach (var language in languages)
            {
                var container = _accordionComponent.HiddenContentContainer;
                var createdLanguageItem = _objectResolver.Instantiate(_languageSelectionItem, container);

                createdLanguageItem.Init(_toggleGroup, language, languageType);
            }
        }
    }
}