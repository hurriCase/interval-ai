using Source.Scripts.UI.Selectables;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards
{
    internal sealed class ControlButtonItem : MonoBehaviour
    {
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}