using Client.Scripts.Core.Localization;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.Localization
{
    /// <summary>
    ///     Adds "Sync" button to LocalizationSync script.
    /// </summary>
    [CustomEditor(typeof(LocalizationSync))]
    public sealed class LocalizationSyncEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var component = (LocalizationSync)target;

            if (GUILayout.Button("Sync"))
                component.Sync();
        }
    }
}