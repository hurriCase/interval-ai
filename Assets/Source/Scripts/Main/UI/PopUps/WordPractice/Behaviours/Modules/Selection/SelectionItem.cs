using Source.Scripts.UI.Components.Checkbox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Selection
{
    internal sealed class SelectionItem : UIBehaviour
    {
        [field: SerializeField] internal CheckboxComponent Checkbox { get; private set; }
        [field: SerializeField] internal ProceduralImage Image { get; private set; }
    }
}