using System;
using Client.Scripts.Editor.EditorCustomization;
using Client.Scripts.UI.Base.Colors;
using Client.Scripts.UI.Base.Theme;
using CustomExtensions.Editor;
using UnityEditor;

namespace Client.Scripts.Editor.UI
{
    [CustomEditor(typeof(ImageThemeComponent), true)]
    internal sealed class ThemeColorComponentEditorBase : EditorBase
    {
        private ThemeColorDatabase ThemeColorDatabase => ThemeColorDatabase.Instance;
        private ThemeHandler ThemeHandler => ThemeHandler.Instance;

        private ImageThemeComponent _imageThemeComponent;

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

            _imageThemeComponent = (ImageThemeComponent)target;

            InitializeColorProperty(nameof(ImageThemeComponent.ThemeSharedColor), ColorType.Shared,
                out _currentSharedColorNameProperty, out _sharedColorIndex);

            InitializeColorProperty(nameof(ImageThemeComponent.ThemeSolidColor), ColorType.SolidColor,
                out _currentSolidColorNameProperty, out _solidColorIndex);

            InitializeColorProperty(nameof(ImageThemeComponent.ThemeGradientColor), ColorType.Gradient,
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
            var colorType = EditorGUILayoutExtensions.DrawEnumField("Color Type", _imageThemeComponent.ColorType);
            _imageThemeComponent.ColorType = (ColorType)colorType;
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
                ThemeHandler.SetTheme(_previewDarkTheme ? ColorTheme.Dark : ColorTheme.Light);
            });
        }

        private void DrawColorSelector()
        {
            var (property, names, index) = GetColorSelectorData(_imageThemeComponent.ColorType);

            EditorGUILayoutExtensions.DrawBoxedSection("Color", () =>
            {
                var newIndex =
                    EditorGUILayoutExtensions.DrawDropdown(nameof(_imageThemeComponent.ColorType), index, names);

                UpdateColorAndPreview(_imageThemeComponent.ColorType, newIndex, property);
            });
        }

        private (SerializedProperty, string[], int) GetColorSelectorData(ColorType colorType) =>
            colorType switch
            {
                ColorType.Shared => (_currentSharedColorNameProperty, ThemeColorDatabase.GetColorNames<SharedColor>(),
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
                    _imageThemeComponent.ThemeSharedColor = sharedColor;
                    _sharedColorIndex = newIndex;
                    colorNameProperty.stringValue = sharedColor.Name;

                    EditorGUILayoutExtensions.DrawColorField("Preview", sharedColor.Color);
                    break;

                case ColorType.SolidColor:
                    var solidColor = ThemeColorDatabase.SolidColors[newIndex];
                    _imageThemeComponent.ThemeSolidColor = solidColor;
                    _solidColorIndex = newIndex;
                    colorNameProperty.stringValue = solidColor.Name;

                    var previewSolidColor = ThemeHandler.CurrentTheme == ColorTheme.Light
                        ? solidColor.LightThemeColor
                        : solidColor.DarkThemeColor;

                    EditorGUILayoutExtensions.DrawColorField("Preview", previewSolidColor);
                    break;

                case ColorType.Gradient:
                    var gradientColor = ThemeColorDatabase.GradientColors[newIndex];
                    _imageThemeComponent.ThemeGradientColor = gradientColor;
                    _gradientColorIndex = newIndex;
                    colorNameProperty.stringValue = gradientColor.Name;

                    var previewGradient = ThemeHandler.CurrentTheme == ColorTheme.Light
                        ? gradientColor.LightThemeColor
                        : gradientColor.DarkThemeColor;

                    EditorGUILayoutExtensions.DrawGradientField("Preview", previewGradient);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}