using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Base.Theme
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    internal sealed class ImageThemeComponent : BaseThemeComponent<Image>
    {
        public override void ApplyColor()
        {
            switch (ColorType)
            {
                case ColorType.Shared:
                    if (_targetComponent.material)
                        _targetComponent.material = null;

                    _targetComponent.color = ThemeSharedColor.Color;
                    break;

                case ColorType.SolidColor:
                    if (_targetComponent.material)
                        _targetComponent.material = null;

                    _targetComponent.color = GetCurrentSolidColor();
                    break;

                case ColorType.Gradient:
                    ApplyGradientToImage(_targetComponent, GetCurrentGradient());
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyGradientToImage(Image targetImage, Gradient gradient, float direction = 0f)
        {
            gradientMaterial ??= new Material(ShaderReferences.Instance.GradientShader);

            targetImage.material = gradientMaterial;

            if (gradientMaterial && gradient.colorKeys.Length >= 2)
            {
                targetImage.color = Color.white;

                gradientMaterial.SetColor(_gradientStartColorProperty, gradient.colorKeys[0].color);
                gradientMaterial.SetColor(_gradientEndColorProperty, gradient.colorKeys[^1].color);
                gradientMaterial.SetFloat(_gradientDirectionProperty, direction);

                targetImage.SetMaterialDirty();
            }
            else if (gradient.colorKeys.Length > 0)
                targetImage.color = gradient.colorKeys[0].color;
        }
    }
}