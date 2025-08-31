using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphTypeItem : MonoBehaviour
    {
        [field: SerializeField] internal ToggleComponent TabComponent { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}