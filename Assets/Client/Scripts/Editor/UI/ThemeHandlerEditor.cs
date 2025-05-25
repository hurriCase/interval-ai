using Client.Scripts.Editor.EditorCustomization;
using Client.Scripts.UI.Theme.Base;
using UnityEditor;

namespace Client.Scripts.Editor.UI
{
    [CustomEditor(typeof(ThemeHandler))]
    internal sealed class ThemeHandlerEditor : EditorBase
    {
        private ThemeHandler _themeHandler;
        private bool _editingLightTheme = true;

        private void OnEnable()
        {
            _themeHandler = target as ThemeHandler;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Current Theme", EditorStyles.boldLabel);

            EditorGUILayoutExtensions.DrawPanel(() =>
            {
                string[] themeLabels = { "Light Theme", "Dark Theme" };
                var selectedTheme = _editingLightTheme ? 0 : 1;

                var newSelectedTheme = EditorGUIExtensions.ToggleButtonGroup(themeLabels, selectedTheme);

                if (newSelectedTheme == selectedTheme)
                    return;

                _editingLightTheme = newSelectedTheme == 0;

                var targetTheme = _editingLightTheme ? ThemeType.Light : ThemeType.Dark;

                if (_themeHandler && _themeHandler.CurrentThemeType != targetTheme)
                    _themeHandler.CurrentThemeType = targetTheme;
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}