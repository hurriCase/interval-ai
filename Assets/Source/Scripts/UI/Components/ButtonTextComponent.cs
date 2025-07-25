using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    internal sealed class ButtonTextComponent : MonoBehaviour
    {
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}