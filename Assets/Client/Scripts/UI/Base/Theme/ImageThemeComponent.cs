using System;
using Client.Scripts.UI.Base.Colors;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.UI.Base.Theme
{
    [RequireComponent(typeof(Image))]
    internal class ImageThemeComponent : MonoBehaviour
    {
        [field: SerializeField] internal bool UseSolidColor { get; private set; } = true;
        [field: SerializeField] internal bool UseGradientColor { get; private set; }
        [field: SerializeField] internal ThemeSolidColor ThemeSolidColor { get; set; }
        [field: SerializeField] internal ThemeGradientColor ThemeGradientColor { get; set; }

        [SerializeField] private Image _targetImage;

        private ThemeHandler ThemeHandler => ThemeHandler.Instance;
        private ColorTheme CurrentTheme => ThemeHandler.CurrentTheme;

        private static readonly int _gradientStartColorProperty = Shader.PropertyToID("_GradientStartColor");
        private static readonly int _gradientEndColorProperty = Shader.PropertyToID("_GradientEndColor");
        private static readonly int _gradientDirectionProperty = Shader.PropertyToID("_GradientDirection");

        private Material _gradientMaterial;

        protected virtual void OnValidate()
        {
            _targetImage ??= GetComponent<Image>();

            ApplyColorToImage();
        }

        protected virtual void Awake()
        {
            ThemeHandler.OnThemeChanged += ApplyColorToImage;
            ApplyColorToImage();
        }

        protected virtual void OnDestroy()
        {
            ThemeHandler.OnThemeChanged -= ApplyColorToImage;

            if (!_gradientMaterial)
                return;

            if (Application.isPlaying)
                Destroy(_gradientMaterial);
            else
                DestroyImmediate(_gradientMaterial);
        }

        private void ApplyColorToImage()
        {
            switch (UseSolidColor)
            {
                case true:
                    if (_targetImage.material)
                        _targetImage.material = null;

                    _targetImage.color = CurrentTheme switch
                    {
                        ColorTheme.Light => ThemeSolidColor.LightThemeColor,
                        ColorTheme.Dark => ThemeSolidColor.DarkThemeColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;

                case false:
                    _gradientMaterial ??= new Material(ShaderReferences.Instance.GradientShader);

                    ApplyGradientToImage(_targetImage);
                    break;
            }
        }

        private void ApplyGradientToImage(Image targetImage, float direction = 0f)
        {
            var gradient = CurrentTheme switch
            {
                ColorTheme.Light => ThemeGradientColor.LightThemeColor,
                ColorTheme.Dark => ThemeGradientColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };

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