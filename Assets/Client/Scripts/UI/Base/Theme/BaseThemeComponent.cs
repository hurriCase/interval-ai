using System;
using Client.Scripts.UI.Base.Colors;
using UnityEngine;

namespace Client.Scripts.UI.Base.Theme
{
    [ExecuteInEditMode]
    internal abstract class BaseThemeComponent<T> : MonoBehaviour, IBaseThemeComponent where T : Component
    {
        [field: SerializeField] public ColorType ColorType { get; set; }
        [field: SerializeField] public ThemeSolidColor ThemeSolidColor { get; set; }
        [field: SerializeField] public ThemeGradientColor ThemeGradientColor { get; set; }
        [field: SerializeField] public SharedColor ThemeSharedColor { get; set; }

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
            return ThemeHandler.CurrentTheme switch
            {
                ColorTheme.Light => ThemeGradientColor.LightThemeColor,
                ColorTheme.Dark => ThemeGradientColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected Color GetCurrentSolidColor()
        {
            return ThemeHandler.CurrentTheme switch
            {
                ColorTheme.Light => ThemeSolidColor.LightThemeColor,
                ColorTheme.Dark => ThemeSolidColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}