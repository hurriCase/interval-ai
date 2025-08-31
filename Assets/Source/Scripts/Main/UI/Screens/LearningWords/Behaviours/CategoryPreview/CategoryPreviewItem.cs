using Source.Scripts.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewItem : MonoBehaviour
    {
        [field: SerializeField] internal Image IconImage { get; private set; }
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
    }
}