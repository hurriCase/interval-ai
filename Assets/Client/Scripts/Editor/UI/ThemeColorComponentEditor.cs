using Client.Scripts.Editor.EditorCustomization;
using Client.Scripts.UI.Base.Colors;
using Client.Scripts.UI.Base.Colors.Base;
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

        private SerializedProperty _useSolidColorProperty;
        private SerializedProperty _useGradientColorProperty;

        private SerializedProperty _themeSolidColorProperty;
        private SerializedProperty _themeGradientColorProperty;
        private SerializedProperty _currentSolidColorNameProperty;
        private SerializedProperty _currentGradientColorNameProperty;

        private string[] _solidColorNames;
        private string[] _gradientColorNames;

        private int _solidColorIndex;
        private int _gradientColorIndex;

        private bool _previewDarkTheme;

        protected override void OnEnable()
        {
            base.OnEnable();

            _useSolidColorProperty = serializedObject.FindField(nameof(ImageThemeComponent.UseSolidColor));
            _useGradientColorProperty = serializedObject.FindField(nameof(ImageThemeComponent.UseGradientColor));
            _themeSolidColorProperty = serializedObject.FindField(nameof(ImageThemeComponent.ThemeSolidColor));
            _themeGradientColorProperty = serializedObject.FindField(nameof(ImageThemeComponent.ThemeGradientColor));

            _solidColorNames = ThemeColorDatabase.GetColorNames<ThemeSolidColor>();
            _gradientColorNames = ThemeColorDatabase.GetColorNames<ThemeGradientColor>();

            _currentSolidColorNameProperty = _themeSolidColorProperty.FindFieldRelative(nameof(ThemeSolidColor.Name));
            var solidColorIndex = ThemeColorDatabase.GetSolidColorIndexByName(_currentSolidColorNameProperty.stringValue);
            _solidColorIndex = solidColorIndex == -1 ? 0 : solidColorIndex;

            _currentGradientColorNameProperty = _themeGradientColorProperty.FindFieldRelative(nameof(ThemeSolidColor.Name));
            var gradientColorIndex =
                ThemeColorDatabase.GetGradientColorIndexByName(_currentGradientColorNameProperty.stringValue);
            _gradientColorIndex = gradientColorIndex == -1 ? 0 : gradientColorIndex;
        }

        protected override void DrawCustomSections()
        {
            DrawFoldoutSection("Theme Settings", () =>
            {
                DrawColorTypeSelectors();
                DrawThemeToggle();
                DrawColorSelectors();
            });
        }

        private void DrawColorTypeSelectors()
        {
            EditorGUILayoutExtensions.DrawPanel(() =>
            {
                EditorGUILayoutExtensions.DrawExclusiveSelection(_useSolidColorProperty, _useGradientColorProperty);

                serializedObject.ApplyModifiedProperties();
            });
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

        private void DrawColorSelectors()
        {
            EditorGUILayoutExtensions.DrawSectionHeader("Color Selection");

            EditorGUILayoutExtensions.DrawPanel(() =>
            {
                if (_useSolidColorProperty.boolValue)
                    DrawColorSelector<ThemeSolidColor>();
                else if (_useGradientColorProperty.boolValue)
                    DrawColorSelector<ThemeGradientColor>();
            });
        }

        private void DrawColorSelector<TColor>() where TColor : IThemeColor
        {
            var isSolidColor = typeof(TColor) == typeof(ThemeSolidColor);
            var fieldName = isSolidColor ? "Color" : "Gradient";
            var colorNameProp = isSolidColor ? _currentSolidColorNameProperty : _currentGradientColorNameProperty;
            var colorNames = isSolidColor ? _solidColorNames : _gradientColorNames;

            var currentIndex = isSolidColor ? _solidColorIndex : _gradientColorIndex;

            EditorGUILayoutExtensions.DrawBoxedSection("Color", () =>
            {
                var newIndex = EditorGUILayoutExtensions.DrawDropdown(fieldName, currentIndex, colorNames);

                var themeComponent = (ImageThemeComponent)target;
                if (isSolidColor)
                {
                    themeComponent.ThemeSolidColor = ThemeColorDatabase.SolidColors[newIndex];
                    _solidColorIndex = newIndex;
                }
                else
                {
                    themeComponent.ThemeGradientColor = ThemeColorDatabase.GradientColors[newIndex];
                    _gradientColorIndex = newIndex;
                }

                colorNameProp.stringValue = isSolidColor switch
                {
                    true when ThemeColorDatabase.SolidColors.Count > newIndex => ThemeColorDatabase
                        .SolidColors[newIndex]
                        .Name,
                    false when ThemeColorDatabase.GradientColors.Count > newIndex => ThemeColorDatabase
                        .GradientColors[newIndex].Name,
                    _ => colorNameProp.stringValue
                };

                switch (isSolidColor)
                {
                    case true:
                    {
                        DrawSolidColorField(newIndex, ThemeHandler.CurrentTheme);
                        break;
                    }
                    case false:
                    {
                        DrawGradientField(newIndex, ThemeHandler.CurrentTheme);
                        break;
                    }
                }
            });
        }

        private void DrawSolidColorField(int newIndex, ColorTheme colorTheme)
        {
            var selectedGradient = ThemeColorDatabase.SolidColors[newIndex];

            ValidateColor(selectedGradient.Name);

            switch (colorTheme)
            {
                case ColorTheme.Light:
                    EditorGUILayoutExtensions.DrawColorField("Preview", selectedGradient.LightThemeColor);
                    break;

                case ColorTheme.Dark:
                    EditorGUILayoutExtensions.DrawColorField("Preview", selectedGradient.DarkThemeColor);
                    break;
            }
        }

        private void DrawGradientField(int newIndex, ColorTheme colorTheme)
        {
            var selectedGradient = ThemeColorDatabase.GradientColors[newIndex];

            ValidateColor(selectedGradient.Name);

            switch (colorTheme)
            {
                case ColorTheme.Light:
                    EditorGUILayoutExtensions.DrawGradientField("Preview", selectedGradient.LightThemeColor);
                    break;

                case ColorTheme.Dark:
                    EditorGUILayoutExtensions.DrawGradientField("Preview", selectedGradient.DarkThemeColor);
                    break;
            }
        }

        private void ValidateColor(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
                EditorGUILayoutExtensions.DrawErrorBox(
                    "This gradient doesn't have a name specified. Gradient names are required for proper theme switching.");
        }
    }
}