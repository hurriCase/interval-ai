using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Answer
{
    internal sealed class ExampleItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI ShownExampleText { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI HiddenExampleText { get; private set; }
    }
}