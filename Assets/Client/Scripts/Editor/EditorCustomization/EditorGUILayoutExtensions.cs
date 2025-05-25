using System;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    /// <summary>
    /// Static utility class for editor layout operations that don't require undo support
    /// </summary>
    internal static class EditorGUILayoutExtensions
    {
        private static ThemeEditorSettings Settings => ThemeEditorSettings.Instance;

        /// <summary>
        /// Draws a section header with consistent styling
        /// </summary>
        internal static void DrawSectionHeader(string title)
        {
            var headerStyle = CreateTextStyle(
                EditorStyles.boldLabel,
                Settings.HeaderFontSize,
                Settings.HeaderFontStyle,
                Settings.HeaderAlignment);

            EditorGUILayout.Space(Settings.HeaderSpacing);
            EditorGUILayout.LabelField(title, headerStyle);
        }

        /// <summary>
        /// Creates a panel with consistent spacing
        /// </summary>
        internal static void DrawPanel(Action drawContent)
        {
            EditorGUILayout.Space(Settings.PanelSpacing);
            drawContent?.Invoke();
            EditorGUILayout.Space(Settings.PanelSpacing);
        }

        /// <summary>
        /// Creates a property field with consistent styling
        /// </summary>
        internal static void DrawPropertyFieldWithLabel(SerializedProperty property, string label)
        {
            EditorGUILayout.PropertyField(property, new GUIContent(label), GUILayout.Height(Settings.PropertyHeight));
        }

        /// <summary>
        /// Creates a group of property fields in a horizontal layout
        /// </summary>
        internal static void HorizontalProperties(params (SerializedProperty property, string label)[] properties)
        {
            EditorGUILayout.BeginHorizontal();

            foreach (var (property, label) in properties)
                DrawPropertyFieldWithLabel(property, label);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Creates a warning message box with consistent styling
        /// </summary>
        internal static void DrawWarningBox(string message)
        {
            EditorGUILayout.Space(Settings.MessageBoxSpacing);
            EditorGUILayout.HelpBox(message, MessageType.Warning);
            EditorGUILayout.Space(Settings.MessageBoxSpacing);
        }

        /// <summary>
        /// Creates an error message box with consistent styling
        /// </summary>
        internal static void DrawErrorBox(string message)
        {
            EditorGUILayout.Space(Settings.MessageBoxSpacing);
            EditorGUILayout.HelpBox(message, MessageType.Error);
            EditorGUILayout.Space(Settings.MessageBoxSpacing);
        }

        /// <summary>
        /// Creates a boxed section with title and content
        /// </summary>
        internal static void DrawBoxedSection(string title, Action drawContent)
        {
            EditorGUILayout.Space(Settings.BoxSpacingBefore);

            var boxStyle = CreateBoxStyle(
                Settings.BoxPaddingLeft,
                Settings.BoxPaddingRight,
                Settings.BoxPaddingTop,
                Settings.BoxPaddingBottom);

            var headerStyle = CreateTextStyle(
                EditorStyles.boldLabel,
                Settings.BoxHeaderFontSize,
                Settings.BoxHeaderFontStyle,
                Settings.BoxHeaderAlignment);

            EditorGUILayout.BeginVertical(boxStyle);

            if (string.IsNullOrEmpty(title) is false)
            {
                EditorGUILayout.Space(Settings.BoxTitleSpacing);
                EditorGUILayout.LabelField(title, headerStyle);
                EditorGUILayout.Space(Settings.BoxTitleSpacing);
            }

            if (drawContent != null)
            {
                EditorGUILayout.Space(Settings.BoxContentSpacing);
                drawContent();
                EditorGUILayout.Space(Settings.BoxContentSpacing);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(Settings.BoxSpacingAfter);
        }

        /// <summary>
        /// Creates a boxed section with foldout header
        /// </summary>
        internal static void DrawBoxWithFoldout(string title, ref bool foldout, Action drawContent)
        {
            EditorGUILayout.Space(Settings.FoldoutBoxSpacingBefore);

            var boxStyle = CreateBoxStyle(
                Settings.FoldoutBoxPaddingLeft,
                Settings.FoldoutBoxPaddingRight,
                Settings.FoldoutBoxPaddingTop,
                Settings.FoldoutBoxPaddingBottom);

            var foldoutStyle = CreateTextStyle(
                EditorStyles.foldout,
                Settings.FoldoutFontSize,
                Settings.FoldoutFontStyle);

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUILayout.Space(Settings.FoldoutHeaderSpacing);

            foldout = EditorGUILayout.Foldout(foldout, title, true, foldoutStyle);

            if (foldout && drawContent != null)
            {
                EditorGUILayout.Space(Settings.FoldoutContentSpacing);
                EditorGUILayout.BeginVertical();
                drawContent();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(Settings.FoldoutContentSpacing);
            }

            EditorGUILayout.Space(Settings.FoldoutHeaderSpacing);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(Settings.FoldoutBoxSpacingAfter);
        }

        /// <summary>
        /// Creates a horizontal line (divider) with consistent styling
        /// </summary>
        internal static void DrawHorizontalLine()
        {
            EditorGUILayout.Space(Settings.DividerSpacing);

            var rect = EditorGUILayout.GetControlRect(false, Settings.DividerHeight);
            rect.height = Settings.DividerHeight;
            EditorGUI.DrawRect(rect, Settings.DividerColor);

            EditorGUILayout.Space(Settings.DividerSpacing);
        }

        /// <summary>
        /// Creates a text style with consistent styling based on settings
        /// </summary>
        internal static GUIStyle CreateTextStyle(GUIStyle baseStyle, int? fontSize = null, FontStyle? fontStyle = null,
            TextAnchor? alignment = null)
        {
            var style = new GUIStyle(baseStyle)
            {
                fontSize = fontSize ?? baseStyle.fontSize,
                fontStyle = fontStyle ?? baseStyle.fontStyle
            };

            if (alignment.HasValue)
                style.alignment = alignment.Value;

            return style;
        }

        /// <summary>
        /// Creates a box style with consistent styling based on settings
        /// </summary>
        internal static GUIStyle CreateBoxStyle(int paddingLeft, int paddingRight, int paddingTop, int paddingBottom)
        {
            var style = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(
                    paddingLeft,
                    paddingRight,
                    paddingTop,
                    paddingBottom)
            };

            return style;
        }
    }
}