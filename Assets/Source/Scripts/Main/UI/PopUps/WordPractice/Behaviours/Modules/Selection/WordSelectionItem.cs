using Source.Scripts.UI.Selectables;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection
{
    internal sealed class WordSelectionItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI Word { get; private set; }
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
    }
}