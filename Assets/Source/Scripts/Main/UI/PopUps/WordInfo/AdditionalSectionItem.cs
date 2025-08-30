using CustomUtils.Runtime.Extensions;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class AdditionalSectionItem : MonoBehaviour
    {
        [field: SerializeField] internal AccordionComponent Accordion { get; set; }
        [field: SerializeField] internal SizeCopierComponent SizeCopier { get; set; }
        [field: SerializeField] internal AspectRatioFitter Spacing { get; set; }
        [field: SerializeField] internal GameObject Separator { get; set; }

        internal void SetActive(bool isActive)
        {
            Accordion.SetActive(isActive);
            Spacing.SetActive(isActive);
            Separator.SetActive(isActive);
        }
    }
}