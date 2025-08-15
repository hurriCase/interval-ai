using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class CheckboxTextComponent : MonoBehaviour
    {
        [field: SerializeField] internal CheckboxComponent Checkbox { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}