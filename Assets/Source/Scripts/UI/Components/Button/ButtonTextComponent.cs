using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Components.Button
{
    internal sealed class ButtonTextComponent : MonoBehaviour
    {
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }

        private void OnValidate()
        {
            Button = GetComponent<ButtonComponent>();
        }
    }
}