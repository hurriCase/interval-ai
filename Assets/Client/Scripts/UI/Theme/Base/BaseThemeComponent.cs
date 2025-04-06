using System;
using Client.Scripts.UI.Theme.ThemeColors;
using UnityEngine;

namespace Client.Scripts.UI.Theme.Base
{
    [ExecuteInEditMode]
    internal abstract class BaseThemeComponent<T> : MonoBehaviour, IBaseThemeComponent where T : Component
    {
        [field: SerializeField] public ColorType ColorType { get; set; } = ColorType.SolidColor;
        [field: SerializeField] public ThemeSolidColor ThemeSolidColor { get; set; }
        [field: SerializeField] public ThemeGradientColor ThemeGradientColor { get; set; }
        [field: SerializeField] public ThemeSharedColor ThemeSharedColor { get; set; }

        [SerializeField] protected T _targetComponent;

        protected static readonly int _gradientStartColorProperty = Shader.PropertyToID("_GradientStartColor");
        protected static readonly int _gradientEndColorProperty = Shader.PropertyToID("_GradientEndColor");
        protected static readonly int _gradientDirectionProperty = Shader.PropertyToID("_GradientDirection");

        protected Material gradientMaterial;

        private ThemeHandler ThemeHandler => ThemeHandler.Instance;

        protected virtual void OnEnable()
        {
            _targetComponent ??= GetComponent<T>();

            ApplyColor();
        }

        protected virtual void OnDestroy()
        {
            if (!gradientMaterial)
                return;

            if (Application.isPlaying)
                Destroy(gradientMaterial);
            else
                DestroyImmediate(gradientMaterial);
        }

        public abstract void ApplyColor();

        protected Gradient GetCurrentGradient()
        {
            return ThemeHandler.CurrentThemeType switch
            {
                ThemeType.Light => ThemeGradientColor.LightThemeColor,
                ThemeType.Dark => ThemeGradientColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected Color GetCurrentSolidColor()
        {
            return ThemeHandler.CurrentThemeType switch
            {
                ThemeType.Light => ThemeSolidColor.LightThemeColor,
                ThemeType.Dark => ThemeSolidColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}