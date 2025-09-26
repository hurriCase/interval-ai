using CustomUtils.Runtime.UI.Theme;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class ThemeLineRenderer : MonoBehaviour
    {
        [field: SerializeField] internal UILineRenderer LineRenderer { get; private set; }
        [field: SerializeField] internal ThemeComponent ThemeComponent { get; private set; }
    }
}