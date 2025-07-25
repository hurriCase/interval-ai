using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphTypeItem : MonoBehaviour
    {
        [field: SerializeField] internal TabComponent TabComponent { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}