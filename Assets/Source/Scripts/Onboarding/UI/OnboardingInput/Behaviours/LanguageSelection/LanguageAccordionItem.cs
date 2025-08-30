using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageAccordionItem : MonoBehaviour
    {
        [field: SerializeField] internal AccordionComponent AccordionComponent { get; private set; }
        [field: SerializeField] internal ToggleGroup ToggleGroup { get; private set; }
    }
}