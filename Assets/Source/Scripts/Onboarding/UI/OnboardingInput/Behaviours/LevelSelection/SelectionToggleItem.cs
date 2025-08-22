using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours.LevelSelection
{
    internal sealed class SelectionToggleItem : MonoBehaviour
    {
        [field: SerializeField] internal Image Icon { get; private set; }
        [field: SerializeField] internal CheckboxComponent CheckboxComponent { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI LevelText { get; private set; }
    }
}