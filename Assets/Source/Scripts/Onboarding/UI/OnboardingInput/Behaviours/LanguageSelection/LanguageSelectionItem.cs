using Source.Scripts.UI.Components.Accordion;
using Source.Scripts.UI.Components.Checkbox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LanguageSelection
{
    internal sealed class LanguageSelectionItem : MonoBehaviour
    {
        [field: SerializeField] internal Image Icon { get; private set; }
        [field: SerializeField] internal AccordionItem AccordionItem { get; private set; }
        [field: SerializeField] internal CheckboxComponent CheckboxComponent { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI LanguageText { get; private set; }
    }
}