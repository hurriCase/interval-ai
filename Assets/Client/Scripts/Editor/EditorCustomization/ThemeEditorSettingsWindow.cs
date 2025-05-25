using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    internal sealed class ThemeEditorSettingsWindow : EditorWindow
    {
        private ThemeEditorSettings Settings => ThemeEditorSettings.Instance;
        private EditorGUIExtensions _editorGUI;

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

        private SerializedObject _serializedObject;

        [MenuItem("Tools/Theme Editor Settings")]
        internal static void ShowWindow()
        {
            var window = GetWindow<ThemeEditorSettingsWindow>("Theme Editor Settings");
            window.Show();
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(Settings);
            _editorGUI ??= new EditorGUIExtensions(Settings);
        }

        private void OnGUI()
        {
            _serializedObject.Update();

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

            _serializedObject.ApplyModifiedProperties();

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
                Settings.HighlightColor =
                    _editorGUI.ColorField("Highlight Color", Settings.HighlightColor);
                Settings.BackgroundColor =
                    _editorGUI.ColorField("Background Color", Settings.BackgroundColor);
                Settings.BorderColor = _editorGUI.ColorField("Border Color", Settings.BorderColor);
            });
        }

        private void DrawHeaderSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Header Settings", ref _headerSettingsFoldout, () =>
            {
                Settings.HeaderSpacing = _editorGUI.FloatField("Spacing", Settings.HeaderSpacing);
                Settings.HeaderFontSize = _editorGUI.IntField("Font Size", Settings.HeaderFontSize);
                Settings.HeaderFontStyle = _editorGUI.EnumField("Font Style", Settings.HeaderFontStyle);
                Settings.HeaderAlignment = _editorGUI.EnumField("Alignment", Settings.HeaderAlignment);
            });
        }

        private void DrawButtonSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Button Settings", ref _buttonSettingsFoldout, () =>
            {
                Settings.ButtonHeight = _editorGUI.FloatField("Height", Settings.ButtonHeight);
                Settings.ButtonFontSize = _editorGUI.IntField("Font Size", Settings.ButtonFontSize);
                Settings.ButtonFontStyle = _editorGUI.EnumField("Font Style", Settings.ButtonFontStyle);
                Settings.ButtonHighlightColor =
                    _editorGUI.ColorField("Highlight Color", Settings.ButtonHighlightColor);
                Settings.ButtonBackgroundColor =
                    _editorGUI.ColorField("Background Color", Settings.ButtonBackgroundColor);
            });
        }

        private void DrawPanelSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Panel Settings", ref _panelSettingsFoldout,
                () =>
                {
                    Settings.PanelSpacing = _editorGUI.FloatField("Spacing", Settings.PanelSpacing);
                });
        }

        private void DrawPropertySettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Property Settings", ref _propertySettingsFoldout, () =>
            {
                Settings.PropertyHeight = _editorGUI.FloatField("Height", Settings.PropertyHeight);
                Settings.PropertyFontSize = _editorGUI.IntField("Font Size", Settings.PropertyFontSize);
                Settings.PropertyFontStyle = _editorGUI.EnumField("Font Style", Settings.PropertyFontStyle);
            });
        }

        private void DrawDropdownSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Dropdown Settings", ref _dropdownSettingsFoldout, () =>
            {
                Settings.DropdownHeight = _editorGUI.FloatField("Height", Settings.DropdownHeight);
                Settings.DropdownFontSize = _editorGUI.IntField("Font Size", Settings.DropdownFontSize);
                Settings.DropdownFontStyle = _editorGUI.EnumField("Font Style", Settings.DropdownFontStyle);
            });
        }

        private void DrawMessageBoxSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Message Box Settings", ref _messageBoxSettingsFoldout,
                () =>
                {
                    Settings.MessageBoxSpacing = _editorGUI.FloatField("Spacing", Settings.MessageBoxSpacing);
                });
        }

        private void DrawColorFieldSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Color Field Settings", ref _colorFieldSettingsFoldout, () =>
            {
                Settings.ColorFieldHeight = _editorGUI.FloatField("Height", Settings.ColorFieldHeight);
                Settings.ColorFieldSpacing = _editorGUI.FloatField("Spacing", Settings.ColorFieldSpacing);
            });
        }

        private void DrawBoxedSectionSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Boxed Section Settings", ref _boxedSectionSettingsFoldout,
                () =>
                {
                    EditorGUILayout.LabelField("Padding", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    Settings.BoxPaddingLeft = _editorGUI.IntField("Left", Settings.BoxPaddingLeft);
                    Settings.BoxPaddingRight = _editorGUI.IntField("Right", Settings.BoxPaddingRight);
                    Settings.BoxPaddingTop = _editorGUI.IntField("Top", Settings.BoxPaddingTop);
                    Settings.BoxPaddingBottom = _editorGUI.IntField("Bottom", Settings.BoxPaddingBottom);

                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space(5);
                    Settings.BoxSpacingBefore = _editorGUI.FloatField("Spacing Before", Settings.BoxSpacingBefore);
                    Settings.BoxSpacingAfter = _editorGUI.FloatField("Spacing After", Settings.BoxSpacingAfter);
                    Settings.BoxTitleSpacing = _editorGUI.FloatField("Title Spacing", Settings.BoxTitleSpacing);
                    Settings.BoxContentSpacing = _editorGUI.FloatField("Content Spacing", Settings.BoxContentSpacing);
                    Settings.BoxHeaderFontSize = _editorGUI.IntField("Header Font Size", Settings.BoxHeaderFontSize);
                    Settings.BoxHeaderFontStyle = _editorGUI.EnumField("Header Font Style", Settings.BoxHeaderFontStyle);
                    Settings.BoxHeaderAlignment = _editorGUI.EnumField("Header Alignment", Settings.BoxHeaderAlignment);
                });
        }

        private void DrawFoldoutSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Foldout Settings", ref _foldoutSettingsFoldout, () =>
            {
                EditorGUILayout.LabelField("Padding", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                Settings.FoldoutBoxPaddingLeft =
                    _editorGUI.IntField("Left", Settings.FoldoutBoxPaddingLeft);
                Settings.FoldoutBoxPaddingRight =
                    _editorGUI.IntField("Right", Settings.FoldoutBoxPaddingRight);
                Settings.FoldoutBoxPaddingTop =
                    _editorGUI.IntField("Top", Settings.FoldoutBoxPaddingTop);
                Settings.FoldoutBoxPaddingBottom =
                    _editorGUI.IntField("Bottom", Settings.FoldoutBoxPaddingBottom);

                EditorGUI.indentLevel--;

                EditorGUILayout.Space(5);
                Settings.FoldoutBoxSpacingBefore =
                    _editorGUI.FloatField("Spacing Before", Settings.FoldoutBoxSpacingBefore);
                Settings.FoldoutBoxSpacingAfter =
                    _editorGUI.FloatField("Spacing After", Settings.FoldoutBoxSpacingAfter);
                Settings.FoldoutHeaderSpacing =
                    _editorGUI.FloatField("Header Spacing", Settings.FoldoutHeaderSpacing);
                Settings.FoldoutContentSpacing =
                    _editorGUI.FloatField("Content Spacing", Settings.FoldoutContentSpacing);
                Settings.FoldoutFontSize = _editorGUI.IntField("Font Size", Settings.FoldoutFontSize);
                Settings.FoldoutFontStyle =
                    _editorGUI.EnumField("Font Style", Settings.FoldoutFontStyle);
            });
        }

        private void DrawDividerSettings()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Divider Settings", ref _dividerSettingsFoldout, () =>
            {
                Settings.DividerHeight = _editorGUI.FloatField("Height", Settings.DividerHeight);
                Settings.DividerSpacing = _editorGUI.FloatField("Spacing", Settings.DividerSpacing);
                Settings.DividerColor = _editorGUI.ColorField("Color", Settings.DividerColor);
            });
        }
    }
}