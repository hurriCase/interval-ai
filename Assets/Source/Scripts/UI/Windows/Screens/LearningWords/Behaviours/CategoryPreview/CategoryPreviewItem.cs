using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.CategoryPreview
{
    internal sealed class CategoryPreviewItem : MonoBehaviour
    {
        [field: SerializeField] internal Image IconImage { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI NameText { get; private set; }
    }
}