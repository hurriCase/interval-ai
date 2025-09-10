using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionItem : MonoBehaviour
    {
        [field: SerializeField] internal AccordionItem AccordionItem { get; private set; }
        [field: SerializeField] internal ToggleComponent Checkbox { get; private set; }
    }
}