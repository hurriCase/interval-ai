using Source.Scripts.UI.Components.Accordion;
using Source.Scripts.UI.Components.Checkbox;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionItem : MonoBehaviour
    {
        [field: SerializeField] internal AccordionItem AccordionItem { get; private set; }
        [field: SerializeField] internal CheckboxComponent Checkbox { get; private set; }
    }
}