using System;
using Client.Scripts.References;
using Client.Scripts.UI.Theme.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Theme.Components
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    internal sealed class ImageThemeComponent : BaseThemeComponent<Image>
    {
        protected override bool ShouldUpdateColor()
        {
            switch (ColorType)
            {
                case ColorType.Shared:
                    return _targetComponent.color != ThemeSharedColor.Color;

                case ColorType.SolidColor:
                    return _targetComponent.color != GetCurrentSolidColor();

                case ColorType.Gradient:
                    _targetComponent.CompareGradient(GetCurrentGradient());
                    return true;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void ApplyColor()
        {
            switch (ColorType)
            {
                case ColorType.Shared:
                    _targetComponent.material = null;
                    _targetComponent.color = ThemeSharedColor.Color;
                    break;

                case ColorType.SolidColor:
                    _targetComponent.material = null;
                    _targetComponent.color = GetCurrentSolidColor();
                    break;

                case ColorType.Gradient:
                    _targetComponent.ApplyGradient(GetCurrentGradient());
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}