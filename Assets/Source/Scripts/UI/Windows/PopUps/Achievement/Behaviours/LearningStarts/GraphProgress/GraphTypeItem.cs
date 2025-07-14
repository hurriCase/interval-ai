using Source.Scripts.UI.Selectables;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphTypeItem : MonoBehaviour
    {
        [field: SerializeField] internal ThemeToggle ThemeToggle { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI Text { get; private set; }
    }
}