using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    [CreateAssetMenu(fileName = nameof(SelectableColorMapping), menuName = nameof(SelectableColorMapping))]
    internal sealed class SelectableColorMapping : ThemeStateMappingGeneric<SelectableStateType>
    {
        internal ColorBlock GetThemeBlockColors(ColorBlock colorBlock)
        {
            colorBlock.normalColor = GetColorForState(SelectableStateType.Normal);
            colorBlock.highlightedColor = GetColorForState(SelectableStateType.Highlighted);
            colorBlock.pressedColor = GetColorForState(SelectableStateType.Pressed);
            colorBlock.selectedColor = GetColorForState(SelectableStateType.Selected);
            colorBlock.disabledColor = GetColorForState(SelectableStateType.Disabled);

            return colorBlock;
        }
    }
}