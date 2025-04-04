using System;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
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
        /// Creates a toggle button with consistent styling
        /// </summary>
        internal static bool DrawToggleButton(string label, bool isSelected, Color? highlightColor = null,
            float? customHeight = null)
        {
            var buttonStyle = CreateTextStyle(
                GUI.skin.button,
                Settings.ButtonFontSize,
                Settings.ButtonFontStyle);

            var originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = isSelected
                ? highlightColor ?? Settings.ButtonHighlightColor
                : Settings.ButtonBackgroundColor;

            var buttonHeight = customHeight ?? Settings.ButtonHeight;
            var clicked = GUILayout.Button(label, buttonStyle, GUILayout.Height(buttonHeight));

            GUI.backgroundColor = originalBackgroundColor;

            return clicked ? !isSelected : isSelected;
        }

        /// <summary>
        /// Creates a group of toggle buttons with consistent styling
        /// </summary>
        internal static int DrawToggleButtonGroup(string[] labels, int selectedIndex, float? customHeight = null)
        {
            EditorGUILayout.BeginHorizontal();

            for (var i = 0; i < labels.Length; i++)
            {
                if (DrawToggleButton(labels[i], selectedIndex == i, null, customHeight))
                    selectedIndex = i;
            }

            EditorGUILayout.EndHorizontal();

            return selectedIndex;
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
        /// Creates a dropdown with consistent styling
        /// </summary>
        internal static int DrawDropdown(string label, int selectedIndex, string[] options, bool indented = true)
        {
            var dropdownStyle = CreateTextStyle(
                EditorStyles.popup,
                Settings.DropdownFontSize,
                Settings.DropdownFontStyle);

            var originalIndent = EditorGUI.indentLevel;

            if (indented)
                EditorGUI.indentLevel++;

            var result = EditorGUILayout.Popup(
                label,
                selectedIndex,
                options,
                dropdownStyle,
                GUILayout.Height(Settings.DropdownHeight)
            );

            EditorGUI.indentLevel = originalIndent;

            return result;
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
        /// Creates a color field
        /// </summary>
        internal static void DrawColorField(string label, Color color)
        {
            EditorGUILayout.Space(Settings.ColorFieldSpacing);
            EditorGUILayout.ColorField(label, color, GUILayout.Height(Settings.ColorFieldHeight));
            EditorGUILayout.Space(Settings.ColorFieldSpacing);
        }

        /// <summary>
        /// Creates a gradient field
        /// </summary>
        internal static Gradient DrawGradientField(string label, Gradient color)
        {
            EditorGUILayout.Space(Settings.ColorFieldSpacing);
            var newGradientValue = EditorGUILayout.GradientField(label, color, GUILayout.Height(Settings.ColorFieldHeight));
            EditorGUILayout.Space(Settings.ColorFieldSpacing);
            return newGradientValue;
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

            if (!string.IsNullOrEmpty(title))
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

        internal static void DrawExclusiveSelection(SerializedProperty leftProperty, SerializedProperty rightProperty)
        {
            EditorGUI.BeginChangeCheck();

            var useSolidColorOld = leftProperty.boolValue;
            var useGradientColorOld = rightProperty.boolValue;

            EditorGUILayout.BeginHorizontal();

            var newUseSolid =
                EditorGUILayout.Toggle(new GUIContent("Use Solid Color"), leftProperty.boolValue);
            var newUseGradient = EditorGUILayout.Toggle(new GUIContent("Use Gradient Color"),
                rightProperty.boolValue);

            EditorGUILayout.EndHorizontal();

            if (newUseSolid == useSolidColorOld && newUseGradient == useGradientColorOld)
                return;

            leftProperty.boolValue = newUseSolid;
            rightProperty.boolValue = newUseGradient;

            switch (newUseSolid)
            {
                case true when newUseGradient:
                    if (useSolidColorOld is false)
                        rightProperty.boolValue = false;
                    else
                        leftProperty.boolValue = false;
                    break;
                case false when newUseGradient is false:
                    if (useSolidColorOld)
                        leftProperty.boolValue = true;
                    else
                        rightProperty.boolValue = true;
                    break;
            }
        }

        private static GUIStyle CreateTextStyle(GUIStyle baseStyle, int? fontSize = null, FontStyle? fontStyle = null,
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

        private static GUIStyle CreateBoxStyle(int paddingLeft, int paddingRight, int paddingTop, int paddingBottom)
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