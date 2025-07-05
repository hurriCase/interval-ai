using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;

namespace Source.Scripts.UI.CustomButton
{
    [CreateAssetMenu(fileName = nameof(ButtonStateColorMapping), menuName = nameof(ButtonStateColorMapping))]
    internal sealed class ButtonStateColorMapping : ThemeStateMappingGeneric<ButtonStateType> { }
}