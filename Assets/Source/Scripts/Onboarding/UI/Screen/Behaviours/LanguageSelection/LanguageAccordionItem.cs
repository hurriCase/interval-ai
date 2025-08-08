using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours.LanguageSelection
{
    internal sealed class LanguageAccordionItem : MonoBehaviour
    {
        [field: SerializeField] internal AccordionComponent AccordionComponent { get; private set; }
        [field: SerializeField] internal ToggleGroup ToggleGroup { get; private set; }
    }
}