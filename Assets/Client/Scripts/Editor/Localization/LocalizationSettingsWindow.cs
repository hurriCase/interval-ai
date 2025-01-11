using Assets.SimpleLocalization.Scripts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Client.Scripts.Editor.Localization
{
    internal class LocalizationSettingsWindow : EditorWindow
    {
        private static SerializedObject _serializedObject;

        private static LocalizationSettings Settings => LocalizationSettings.Instance;

        [MenuItem("Project/Localization/Settings")]
        internal static void ShowWindow()
        {
            GetWindow<LocalizationSettingsWindow>("Localization Settings");
        }

        [MenuItem("Project/Localization/Reset")]
        internal static void ResetSettings()
        {
            if (EditorUtility.DisplayDialog(
                    "Simple Localization",
                    "Do you want to reset settings?",
                    "Yes",
                    "No"))
                LocalizationSettings.Instance.Reset();
        }

        private void MakeSettingsWindow()
        {
            minSize = new Vector2(300, 500);
            Settings.DisplayHelp();
            Settings.TableId = EditorGUILayout.TextField("Table Id", Settings.TableId, GUILayout.MinWidth(200));
            DisplaySheets();
            Settings.SaveFolder =
                EditorGUILayout.ObjectField("Save Folder", Settings.SaveFolder, typeof(Object), false);
            Settings.DisplayButtons();
            Settings.DisplayWarnings();
        }

        private static void DisplaySheets()
        {
            if (_serializedObject == null || _serializedObject.targetObject is null)
                _serializedObject = new SerializedObject(Settings);
            else
                _serializedObject.Update();

            var property = _serializedObject.FindProperty("Sheets");

            EditorGUILayout.PropertyField(property, new GUIContent("Sheets"), true);

            if (property.isArray)
            {
                property.Next(true);
                property.Next(true);

                var length = property.intValue;

                property.Next(true);

                Settings.Sheets.Clear();

                var lastIndex = length - 1;

                for (var i = 0; i < length; i++)
                {
                    Settings.Sheets.Add(new Sheet
                    {
                        Name = property.FindPropertyRelative("Name").stringValue,
                        Id = property.FindPropertyRelative("Id").longValue,
                        TextAsset = property.FindPropertyRelative("TextAsset").objectReferenceValue as TextAsset
                    });

                    if (i < lastIndex)
                        property.Next(false);
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }

        private void OnGUI()
        {
            MakeSettingsWindow();
        }
    }
}