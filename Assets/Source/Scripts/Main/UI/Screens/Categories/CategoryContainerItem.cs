using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.Categories
{
    internal sealed class CategoryContainerItem : MonoBehaviour
    {
        [field: SerializeField] internal TextMeshProUGUI TitleText { get; private set; }
        [field: SerializeField] internal RectTransform CategoryContainer { get; private set; }
    }
}