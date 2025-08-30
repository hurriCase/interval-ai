using UnityEngine;

namespace Source.Scripts.UI.Components.Accordion
{
    [RequireComponent(typeof(CanvasGroup))]
    internal class AccordionItem : MonoBehaviour
    {
        [field: SerializeField] internal CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] internal RectTransform RectTransform { get; private set; }

        private void OnValidate()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }
    }
}