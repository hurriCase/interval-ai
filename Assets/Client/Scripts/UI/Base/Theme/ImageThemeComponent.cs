using System;
using Client.Scripts.UI.Base.Colors;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Base.Theme
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    internal class ImageThemeComponent : MonoBehaviour
    {
        [field: SerializeField] internal ColorType ColorType { get; set; }
        [field: SerializeField] internal ThemeSolidColor ThemeSolidColor { get; set; }
        [field: SerializeField] internal ThemeGradientColor ThemeGradientColor { get; set; }
        [field: SerializeField] internal SharedColor ThemeSharedColor { get; set; }

        [SerializeField] private Image _targetImage;

        private ThemeHandler ThemeHandler => ThemeHandler.Instance;

        private static readonly int _gradientStartColorProperty = Shader.PropertyToID("_GradientStartColor");
        private static readonly int _gradientEndColorProperty = Shader.PropertyToID("_GradientEndColor");
        private static readonly int _gradientDirectionProperty = Shader.PropertyToID("_GradientDirection");

        private Material _gradientMaterial;

        protected virtual void OnEnable()
        {
            _targetImage ??= GetComponent<Image>();

            ApplyColorToImage();
        }

        protected virtual void OnDestroy()
        {
            if (!_gradientMaterial)
                return;

            if (Application.isPlaying)
                Destroy(_gradientMaterial);
            else
                DestroyImmediate(_gradientMaterial);
        }

        internal void ApplyColorToImage()
        {
            switch (ColorType)
            {
                case ColorType.Shared:
                    if (_targetImage.material)
                        _targetImage.material = null;

                    _targetImage.color = ThemeSharedColor.Color;
                    break;

                case ColorType.SolidColor:
                    if (_targetImage.material)
                        _targetImage.material = null;

                    _targetImage.color = ThemeHandler.CurrentTheme switch
                    {
                        ColorTheme.Light => ThemeSolidColor.LightThemeColor,
                        ColorTheme.Dark => ThemeSolidColor.DarkThemeColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;

                case ColorType.Gradient:
                    var gradient = ThemeHandler.CurrentTheme switch
                    {
                        ColorTheme.Light => ThemeGradientColor.LightThemeColor,
                        ColorTheme.Dark => ThemeGradientColor.DarkThemeColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    ApplyGradientToImage(_targetImage, gradient);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyGradientToImage(Image targetImage, Gradient gradient, float direction = 0f)
        {
            _gradientMaterial ??= new Material(ShaderReferences.Instance.GradientShader);

            targetImage.material = _gradientMaterial;

            if (_gradientMaterial && gradient.colorKeys.Length >= 2)
            {
                targetImage.color = Color.white;

                _gradientMaterial.SetColor(_gradientStartColorProperty, gradient.colorKeys[0].color);
                _gradientMaterial.SetColor(_gradientEndColorProperty, gradient.colorKeys[^1].color);
                _gradientMaterial.SetFloat(_gradientDirectionProperty, direction);

                targetImage.SetMaterialDirty();
            }
            else if (gradient.colorKeys.Length > 0)
                targetImage.color = gradient.colorKeys[0].color;
        }
    }
}