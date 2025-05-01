﻿using System;
using Client.Scripts.UI.Theme.ThemeColors;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Client.Scripts.UI.Theme.Base
{
    [ExecuteInEditMode]
    internal abstract class BaseThemeComponent<T> : MonoBehaviour, IBaseThemeComponent where T : Component
    {
        [field: SerializeField] public ColorType ColorType { get; set; } = ColorType.SolidColor;
        [field: SerializeField] public ThemeSharedColor ThemeSharedColor { get; set; }
        [field: SerializeField] public ThemeSolidColor ThemeSolidColor { get; set; }
        [field: SerializeField] public ThemeGradientColor ThemeGradientColor { get; set; }

        [SerializeField] protected T _targetComponent;
        private ThemeHandler ThemeHandler => ThemeHandler.Instance;
        private ThemeColorDatabase ThemeColorDatabase => ThemeColorDatabase.Instance;

        private ThemeSharedColor _previousSharedThemeColor;
        private ThemeSolidColor _previousSolidThemeColor;
        private ThemeGradientColor _previousGradientThemeColor;
        private ColorType _previousColorType;
        private ThemeType _previousThemeType;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (ThemeColorDatabase.SharedColor is { Count: > 0 })
                ThemeSharedColor = ThemeColorDatabase.SharedColor[0];

            if (ThemeColorDatabase.SolidColors is { Count: > 0 })
                ThemeSolidColor = ThemeColorDatabase.SolidColors[0];

            if (ThemeColorDatabase.GradientColors is { Count: > 0 })
                ThemeGradientColor = ThemeColorDatabase.GradientColors[0];

            OnApplyColor();
        }
#endif

        protected virtual void OnEnable()
        {
            _targetComponent ??= GetComponent<T>();

            OnApplyColor();
        }

        public virtual void OnApplyColor()
        {
            bool colorChanged;

            switch (ColorType)
            {
                case ColorType.Shared:
                    colorChanged = _previousSharedThemeColor != ThemeSharedColor;
                    if (colorChanged)
                        _previousSharedThemeColor = ThemeSharedColor;
                    break;

                case ColorType.SolidColor:
                    colorChanged = _previousSolidThemeColor != ThemeSolidColor;
                    if (colorChanged)
                        _previousSolidThemeColor = ThemeSolidColor;
                    break;

                case ColorType.Gradient:
                    colorChanged = _previousGradientThemeColor != ThemeGradientColor;
                    if (colorChanged)
                        _previousGradientThemeColor = ThemeGradientColor;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (colorChanged is false && _previousThemeType == ThemeHandler.CurrentThemeType &&
                _previousColorType == ColorType && ShouldUpdateColor() is false)
                return;

            ApplyColor();

            _previousThemeType = ThemeHandler.CurrentThemeType;
            _previousColorType = ColorType;
        }

        protected abstract bool ShouldUpdateColor();

        protected abstract void ApplyColor();

        protected Gradient GetCurrentGradient()
        {
            if (ThemeGradientColor == null)
                return null;

            return ThemeHandler.CurrentThemeType switch
            {
                ThemeType.Light => ThemeGradientColor.LightThemeColor,
                ThemeType.Dark => ThemeGradientColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected Color GetCurrentSolidColor()
        {
            if (ThemeSolidColor == null)
                return Color.white;

            return ThemeHandler.CurrentThemeType switch
            {
                ThemeType.Light => ThemeSolidColor.LightThemeColor,
                ThemeType.Dark => ThemeSolidColor.DarkThemeColor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}