using System;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.EditorCustomization
{
    /// <summary>
    /// Enhanced GUI system for editor inspectors with automatic undo support
    /// </summary>
    internal sealed class EditorGUIExtensions
    {
        private static ThemeEditorSettings Settings => ThemeEditorSettings.Instance;
        private readonly UnityEngine.Object _target;

        internal EditorGUIExtensions(UnityEngine.Object target) =>
            _target = target ?? throw new ArgumentNullException(nameof(target));

        // Generic method to handle value changes with undo support
        private T HandleValueChange<T>(string label, T currentValue, Func<T> guiMethod)
        {
            EditorGUI.BeginChangeCheck();
            var newValue = guiMethod();
            if (EditorGUI.EndChangeCheck() is false)
                return currentValue;

            Undo.RecordObject(_target, $"Change {label}");
            return newValue;
        }

        /// <summary>
        /// Creates a color field with undo support
        /// </summary>
        internal Color ColorField(string label, Color value) =>
            HandleValueChange(label, value, () => EditorGUILayout.ColorField(label, value));

        /// <summary>
        /// Creates a color field with consistent styling and undo support
        /// </summary>
        internal Color ColorField(string label, Color value, bool useConsistentHeight) =>
            HandleValueChange(label, value, () =>
                useConsistentHeight
                    ? EditorGUILayout.ColorField(label, value, GUILayout.Height(Settings.ColorFieldHeight))
                    : EditorGUILayout.ColorField(label, value));

        /// <summary>
        /// Creates a gradient field with undo support
        /// </summary>
        internal Gradient GradientField(string label, Gradient value) =>
            HandleValueChange(label, value, () => EditorGUILayout.GradientField(label, value));

        /// <summary>
        /// Creates a gradient field with consistent styling and undo support
        /// </summary>
        internal Gradient GradientField(string label, Gradient value, bool useConsistentHeight) =>
            HandleValueChange(label, value, () =>
                useConsistentHeight
                    ? EditorGUILayout.GradientField(label, value, GUILayout.Height(Settings.ColorFieldHeight))
                    : EditorGUILayout.GradientField(label, value));

        /// <summary>
        /// Creates a float field with undo support
        /// </summary>
        internal float FloatField(string label, float value) =>
            HandleValueChange(label, value, () => EditorGUILayout.FloatField(label, value));

        /// <summary>
        /// Creates an int field with undo support
        /// </summary>
        internal int IntField(string label, int value) =>
            HandleValueChange(label, value, () => EditorGUILayout.IntField(label, value));

        /// <summary>
        /// Creates an enum field with undo support
        /// </summary>
        internal T EnumField<T>(string label, T value) where T : Enum =>
            HandleValueChange(label, value, () => (T)EditorGUILayout.EnumPopup(label, value));

        /// <summary>
        /// Creates a dropdown with undo support
        /// </summary>
        internal int Dropdown(string label, int selectedIndex, string[] options, bool indented = true)
        {
            var dropdownStyle = EditorGUILayoutExtensions.CreateTextStyle(
                EditorStyles.popup,
                Settings.DropdownFontSize,
                Settings.DropdownFontStyle);

            var originalIndent = EditorGUI.indentLevel;
            if (indented)
                EditorGUI.indentLevel++;

            var result = HandleValueChange(label, selectedIndex, () =>
                EditorGUILayout.Popup(
                    label,
                    selectedIndex,
                    options,
                    dropdownStyle,
                    GUILayout.Height(Settings.DropdownHeight)
                ));

            EditorGUI.indentLevel = originalIndent;
            return result;
        }

        /// <summary>
        /// Creates a toggle button with undo support
        /// </summary>
        internal bool ToggleButton(string label, bool isSelected, Color? highlightColor = null)
        {
            var buttonStyle = EditorGUILayoutExtensions.CreateTextStyle(
                GUI.skin.button,
                Settings.ButtonFontSize,
                Settings.ButtonFontStyle);

            var originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = isSelected
                ? highlightColor ?? Settings.ButtonHighlightColor
                : Settings.ButtonBackgroundColor;

            EditorGUI.BeginChangeCheck();
            var clicked = GUILayout.Button(label, buttonStyle, GUILayout.Height(Settings.ButtonHeight));

            GUI.backgroundColor = originalBackgroundColor;

            if (EditorGUI.EndChangeCheck() is false || clicked is false)
                return isSelected;

            Undo.RecordObject(_target, $"Toggle {label}");
            return isSelected is false;
        }

        /// <summary>
        /// Creates a toggle button group with undo support
        /// </summary>
        internal int ToggleButtonGroup(string[] labels, int selectedIndex)
        {
            EditorGUILayout.BeginHorizontal();

            var newIndex = selectedIndex;

            for (var i = 0; i < labels.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                var isSelected = selectedIndex == i;
                var newIsSelected = ToggleButton(labels[i], isSelected);

                if (EditorGUI.EndChangeCheck() && newIsSelected && isSelected is false)
                    newIndex = i;
            }

            EditorGUILayout.EndHorizontal();

            return newIndex;
        }

        /// <summary>
        /// Creates a toggle with undo support
        /// </summary>
        internal bool Toggle(string label, bool value) =>
            HandleValueChange(label, value, () => EditorGUILayout.Toggle(label, value));

        /// <summary>
        /// Creates exclusive toggles (one must be true) with undo support
        /// </summary>
        internal void ExclusiveToggles(ref bool toggle1, ref bool toggle2, string label1, string label2)
        {
            EditorGUI.BeginChangeCheck();

            var oldToggle1 = toggle1;
            var oldToggle2 = toggle2;

            EditorGUILayout.BeginHorizontal();

            var newToggle1 = EditorGUILayout.Toggle(new GUIContent(label1), toggle1);
            var newToggle2 = EditorGUILayout.Toggle(new GUIContent(label2), toggle2);

            EditorGUILayout.EndHorizontal();

            if (newToggle1 == oldToggle1 && newToggle2 == oldToggle2)
                return;

            if (EditorGUI.EndChangeCheck() is false)
                return;

            Undo.RecordObject(_target, $"Change {label1}/{label2} Selection");

            toggle1 = newToggle1;
            toggle2 = newToggle2;

            switch (newToggle1)
            {
                case true when newToggle2:
                    if (oldToggle1 == false)
                        toggle2 = false;
                    else
                        toggle1 = false;
                    break;
                case false when newToggle2 == false:
                    if (oldToggle1)
                        toggle1 = true;
                    else
                        toggle2 = true;
                    break;
            }
        }

        /// <summary>
        /// Creates a text area with undo support
        /// </summary>
        internal string TextArea(string label, string value)
        {
            EditorGUILayout.LabelField(label);
            return HandleValueChange(label, value, () => EditorGUILayout.TextArea(value));
        }

        /// <summary>
        /// Creates a text field with undo support
        /// </summary>
        internal string TextField(string label, string value) =>
            HandleValueChange(label, value, () => EditorGUILayout.TextField(label, value));

        /// <summary>
        /// Creates a slider with undo support
        /// </summary>
        internal float Slider(string label, float value, float leftValue, float rightValue) =>
            HandleValueChange(label, value, () => EditorGUILayout.Slider(label, value, leftValue, rightValue));

        /// <summary>
        /// Creates an int slider with undo support
        /// </summary>
        internal int IntSlider(string label, int value, int leftValue, int rightValue) =>
            HandleValueChange(label, value, () => EditorGUILayout.IntSlider(label, value, leftValue, rightValue));
    }
}