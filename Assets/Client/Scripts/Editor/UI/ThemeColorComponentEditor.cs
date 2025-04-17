using System;
using Client.Scripts.Editor.EditorCustomization;
using Client.Scripts.UI.Theme.Base;
using Client.Scripts.UI.Theme.ThemeColors;
using CustomExtensions.Editor;
using UnityEditor;

namespace Client.Scripts.Editor.UI
{
    [CustomEditor(typeof(BaseThemeComponent<>), true)]
    internal sealed class ThemeColorComponentEditorBase : EditorBase
    {
        private ThemeColorDatabase ThemeColorDatabase => ThemeColorDatabase.Instance;
        private ThemeHandler ThemeHandler => ThemeHandler.Instance;

        private IBaseThemeComponent _themeComponent;

        private SerializedProperty _currentSharedColorNameProperty;
        private SerializedProperty _currentSolidColorNameProperty;
        private SerializedProperty _currentGradientColorNameProperty;
        private int _solidColorIndex;
        private int _gradientColorIndex;
        private int _sharedColorIndex;
        private bool _previewDarkTheme;

        protected override void OnEnable()
        {
            base.OnEnable();

            _themeComponent = target as IBaseThemeComponent;

            if (_themeComponent == null)
                return;

            InitializeColorProperty("ThemeSharedColor", ColorType.Shared,
                out _currentSharedColorNameProperty, out _sharedColorIndex);

            InitializeColorProperty("ThemeSolidColor", ColorType.SolidColor,
                out _currentSolidColorNameProperty, out _solidColorIndex);

            InitializeColorProperty("ThemeGradientColor", ColorType.Gradient,
                out _currentGradientColorNameProperty, out _gradientColorIndex);
        }

        private void InitializeColorProperty(string propertyName, ColorType colorType,
            out SerializedProperty colorNameProperty, out int colorIndex)
        {
            var colorProperty = serializedObject.FindField(propertyName);
            colorNameProperty = colorProperty.FindFieldRelative(nameof(IThemeColor.Name));
            var currentColorIndex = ThemeColorDatabase.GetColorIndexByName(colorNameProperty.stringValue, colorType);
            colorIndex = currentColorIndex == -1 ? 0 : currentColorIndex;
        }

        protected override void DrawCustomSections()
        {
            DrawFoldoutSection("Theme Settings", () =>
            {
                DrawColorTypeProperty();
                DrawThemeToggle();
                DrawColorSelector();
            });
        }

        private void DrawColorTypeProperty()
        {
            var colorType = (ColorType)EditorGUILayoutExtensions.DrawEnumField("Color Type", _themeComponent.ColorType);

            if (_themeComponent.ColorType == colorType)
                return;

            _themeComponent.ColorType = colorType;
            _themeComponent.OnApplyColor();

            EditorUtility.SetDirty(target);
        }

        private void DrawThemeToggle()
        {
            EditorGUILayoutExtensions.DrawPanel(() =>
            {
                string[] themeLabels = { "Light Theme", "Dark Theme" };
                var selectedTheme = _previewDarkTheme ? 1 : 0;
                var newSelectedTheme = EditorGUILayoutExtensions.DrawToggleButtonGroup(themeLabels, selectedTheme);

                if (newSelectedTheme == selectedTheme)
                    return;

                _previewDarkTheme = newSelectedTheme == 1;
                ThemeHandler.CurrentThemeType = _previewDarkTheme ? ThemeType.Dark : ThemeType.Light;

                _themeComponent.OnApplyColor();
                EditorUtility.SetDirty(target);
            });
        }

        private void DrawColorSelector()
        {
            var (property, names, index) = GetColorSelectorData(_themeComponent.ColorType);

            EditorGUILayoutExtensions.DrawBoxedSection("Color", () =>
            {
                var newIndex =
                    EditorGUILayoutExtensions.DrawDropdown(nameof(_themeComponent.ColorType), index, names);

                if (newIndex != index)
                    UpdateColorAndPreview(_themeComponent.ColorType, newIndex, property);

                switch (_themeComponent.ColorType)
                {
                    case ColorType.Shared:
                        EditorGUILayoutExtensions.DrawColorField("Preview", _themeComponent.ThemeSharedColor.Color);
                        break;
                    case ColorType.SolidColor:
                        var previewSolidColor = ThemeHandler.CurrentThemeType == ThemeType.Light
                            ? _themeComponent.ThemeSolidColor.LightThemeColor
                            : _themeComponent.ThemeSolidColor.DarkThemeColor;

                        EditorGUILayoutExtensions.DrawColorField("Preview", previewSolidColor);
                        break;
                    case ColorType.Gradient:
                        var previewGradient = ThemeHandler.CurrentThemeType == ThemeType.Light
                            ? _themeComponent.ThemeGradientColor.LightThemeColor
                            : _themeComponent.ThemeGradientColor.DarkThemeColor;

                        EditorGUILayoutExtensions.DrawGradientField("Preview", previewGradient);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private (SerializedProperty, string[], int) GetColorSelectorData(ColorType colorType) =>
            colorType switch
            {
                ColorType.Shared => (_currentSharedColorNameProperty, ThemeColorDatabase.GetColorNames<ThemeSharedColor>(),
                    _sharedColorIndex),
                ColorType.SolidColor => (_currentSolidColorNameProperty,
                    ThemeColorDatabase.GetColorNames<ThemeSolidColor>(), _solidColorIndex),
                ColorType.Gradient => (_currentGradientColorNameProperty,
                    ThemeColorDatabase.GetColorNames<ThemeGradientColor>(), _gradientColorIndex),
                _ => throw new ArgumentOutOfRangeException()
            };

        private void UpdateColorAndPreview(ColorType colorType, int newIndex, SerializedProperty colorNameProperty)
        {
            switch (colorType)
            {
                case ColorType.Shared:
                    var sharedColor = ThemeColorDatabase.SharedColor[newIndex];
                    _themeComponent.ThemeSharedColor = sharedColor;
                    _sharedColorIndex = newIndex;
                    colorNameProperty.stringValue = sharedColor.Name;
                    break;

                case ColorType.SolidColor:
                    var solidColor = ThemeColorDatabase.SolidColors[newIndex];
                    _themeComponent.ThemeSolidColor = solidColor;
                    _solidColorIndex = newIndex;
                    colorNameProperty.stringValue = solidColor.Name;
                    break;

                case ColorType.Gradient:
                    var gradientColor = ThemeColorDatabase.GradientColors[newIndex];
                    _themeComponent.ThemeGradientColor = gradientColor;
                    _gradientColorIndex = newIndex;
                    colorNameProperty.stringValue = gradientColor.Name;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _themeComponent.OnApplyColor();
            EditorUtility.SetDirty(target);
        }
    }
}