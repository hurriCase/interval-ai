using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    /// <summary>
    /// Custom window for editing theme editor settings
    /// </summary>
    internal sealed class ThemeEditorSettingsWindow : EditorWindow
    {
        private ThemeEditorSettings Settings => ThemeEditorSettings.Instance;
        private Vector2 _scrollPosition;

        private bool _globalSettingsFoldout = true;
        private bool _headerSettingsFoldout;
        private bool _buttonSettingsFoldout;
        private bool _panelSettingsFoldout;
        private bool _propertySettingsFoldout;
        private bool _dropdownSettingsFoldout;
        private bool _messageBoxSettingsFoldout;
        private bool _colorFieldSettingsFoldout;
        private bool _boxedSectionSettingsFoldout;
        private bool _foldoutSettingsFoldout;
        private bool _dividerSettingsFoldout;

        [MenuItem("Tools/Theme Editor Settings")]
        internal static void ShowWindow()
        {
            var window = GetWindow<ThemeEditorSettingsWindow>("Theme Editor Settings");
            window.Show();
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.LabelField("Theme Editor Settings", EditorStyles.boldLabel);

            DrawGlobalSettings();
            DrawHeaderSettings();
            DrawButtonSettings();
            DrawPanelSettings();
            DrawPropertySettings();
            DrawDropdownSettings();
            DrawMessageBoxSettings();
            DrawColorFieldSettings();
            DrawBoxedSectionSettings();
            DrawFoldoutSettings();
            DrawDividerSettings();

            EditorGUILayout.EndScrollView();

            if (GUI.changed is false)
                return;

            EditorUtility.SetDirty(Settings);
            AssetDatabase.SaveAssets();
        }

        private void DrawGlobalSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Global Settings", ref _globalSettingsFoldout, () =>
            {
                EditorGUILayout.LabelField("Global Colors", EditorStyles.boldLabel);
                Settings.HighlightColor = EditorGUILayout.ColorField("Highlight Color", Settings.HighlightColor);
                Settings.BackgroundColor = EditorGUILayout.ColorField("Background Color", Settings.BackgroundColor);
                Settings.BorderColor = EditorGUILayout.ColorField("Border Color", Settings.BorderColor);
            });
        }

        private void DrawHeaderSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Header Settings", ref _headerSettingsFoldout, () =>
            {
                Settings.HeaderSpacing = EditorGUILayout.FloatField("Spacing", Settings.HeaderSpacing);
                Settings.HeaderFontSize = EditorGUILayout.IntField("Font Size", Settings.HeaderFontSize);
                Settings.HeaderFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", Settings.HeaderFontStyle);
                Settings.HeaderAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Alignment", Settings.HeaderAlignment);
            });
        }

        private void DrawButtonSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Button Settings", ref _buttonSettingsFoldout, () =>
            {
                Settings.ButtonHeight = EditorGUILayout.FloatField("Height", Settings.ButtonHeight);
                Settings.ButtonFontSize = EditorGUILayout.IntField("Font Size", Settings.ButtonFontSize);
                Settings.ButtonFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", Settings.ButtonFontStyle);
                Settings.ButtonHighlightColor = EditorGUILayout.ColorField("Highlight Color", Settings.ButtonHighlightColor);
                Settings.ButtonBackgroundColor = EditorGUILayout.ColorField("Background Color", Settings.ButtonBackgroundColor);
            });
        }

        private void DrawPanelSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Panel Settings", ref _panelSettingsFoldout, () =>
            {
                Settings.PanelSpacing = EditorGUILayout.FloatField("Spacing", Settings.PanelSpacing);
            });
        }

        private void DrawPropertySettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Property Settings", ref _propertySettingsFoldout, () =>
            {
                Settings.PropertyHeight = EditorGUILayout.FloatField("Height", Settings.PropertyHeight);
                Settings.PropertyFontSize = EditorGUILayout.IntField("Font Size", Settings.PropertyFontSize);
                Settings.PropertyFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", Settings.PropertyFontStyle);
            });
        }

        private void DrawDropdownSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Dropdown Settings", ref _dropdownSettingsFoldout, () =>
            {
                Settings.DropdownHeight = EditorGUILayout.FloatField("Height", Settings.DropdownHeight);
                Settings.DropdownFontSize = EditorGUILayout.IntField("Font Size", Settings.DropdownFontSize);
                Settings.DropdownFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", Settings.DropdownFontStyle);
            });
        }

        private void DrawMessageBoxSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Message Box Settings", ref _messageBoxSettingsFoldout, () =>
            {
                Settings.MessageBoxSpacing = EditorGUILayout.FloatField("Spacing", Settings.MessageBoxSpacing);
            });
        }

        private void DrawColorFieldSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Color Field Settings", ref _colorFieldSettingsFoldout, () =>
            {
                Settings.ColorFieldHeight = EditorGUILayout.FloatField("Height", Settings.ColorFieldHeight);
                Settings.ColorFieldSpacing = EditorGUILayout.FloatField("Spacing", Settings.ColorFieldSpacing);
            });
        }

        private void DrawBoxedSectionSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Boxed Section Settings", ref _boxedSectionSettingsFoldout, () =>
            {
                EditorGUILayout.LabelField("Padding", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                Settings.BoxPaddingLeft = EditorGUILayout.IntField("Left", Settings.BoxPaddingLeft);
                Settings.BoxPaddingRight = EditorGUILayout.IntField("Right", Settings.BoxPaddingRight);
                Settings.BoxPaddingTop = EditorGUILayout.IntField("Top", Settings.BoxPaddingTop);
                Settings.BoxPaddingBottom = EditorGUILayout.IntField("Bottom", Settings.BoxPaddingBottom);
                EditorGUI.indentLevel--;

                EditorGUILayout.Space(5);
                Settings.BoxSpacingBefore = EditorGUILayout.FloatField("Spacing Before", Settings.BoxSpacingBefore);
                Settings.BoxSpacingAfter = EditorGUILayout.FloatField("Spacing After", Settings.BoxSpacingAfter);
                Settings.BoxTitleSpacing = EditorGUILayout.FloatField("Title Spacing", Settings.BoxTitleSpacing);
                Settings.BoxContentSpacing = EditorGUILayout.FloatField("Content Spacing", Settings.BoxContentSpacing);
                Settings.BoxHeaderFontSize = EditorGUILayout.IntField("Header Font Size", Settings.BoxHeaderFontSize);
                Settings.BoxHeaderFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Header Font Style", Settings.BoxHeaderFontStyle);
                Settings.BoxHeaderAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Header Alignment", Settings.BoxHeaderAlignment);
            });
        }

        private void DrawFoldoutSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Foldout Settings", ref _foldoutSettingsFoldout, () =>
            {
                EditorGUILayout.LabelField("Padding", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                Settings.FoldoutBoxPaddingLeft = EditorGUILayout.IntField("Left", Settings.FoldoutBoxPaddingLeft);
                Settings.FoldoutBoxPaddingRight = EditorGUILayout.IntField("Right", Settings.FoldoutBoxPaddingRight);
                Settings.FoldoutBoxPaddingTop = EditorGUILayout.IntField("Top", Settings.FoldoutBoxPaddingTop);
                Settings.FoldoutBoxPaddingBottom = EditorGUILayout.IntField("Bottom", Settings.FoldoutBoxPaddingBottom);
                EditorGUI.indentLevel--;

                EditorGUILayout.Space(5);
                Settings.FoldoutBoxSpacingBefore = EditorGUILayout.FloatField("Spacing Before", Settings.FoldoutBoxSpacingBefore);
                Settings.FoldoutBoxSpacingAfter = EditorGUILayout.FloatField("Spacing After", Settings.FoldoutBoxSpacingAfter);
                Settings.FoldoutHeaderSpacing = EditorGUILayout.FloatField("Header Spacing", Settings.FoldoutHeaderSpacing);
                Settings.FoldoutContentSpacing = EditorGUILayout.FloatField("Content Spacing", Settings.FoldoutContentSpacing);
                Settings.FoldoutFontSize = EditorGUILayout.IntField("Font Size", Settings.FoldoutFontSize);
                Settings.FoldoutFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", Settings.FoldoutFontStyle);
            });
        }

        private void DrawDividerSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Divider Settings", ref _dividerSettingsFoldout, () =>
            {
                Settings.DividerHeight = EditorGUILayout.FloatField("Height", Settings.DividerHeight);
                Settings.DividerSpacing = EditorGUILayout.FloatField("Spacing", Settings.DividerSpacing);
                Settings.DividerColor = EditorGUILayout.ColorField("Color", Settings.DividerColor);
            });
        }
    }
}