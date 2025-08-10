using UnityEngine;

namespace Source.Scripts.UI.Components
{
    [RequireComponent(typeof(CanvasGroup))]
    internal sealed class AccordionItem : MonoBehaviour
    {
        [field: SerializeField] internal CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] internal RectTransform RectTransform { get; private set; }
    }
}