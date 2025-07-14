using CustomUtils.Editor.CustomEditorUtilities;
using Source.Scripts.UI.Selectables;
using UnityEditor;
using UnityEditor.UI;

namespace Source.Scripts.Editor
{
    [CustomEditor(typeof(SwitchableToggle))]
    internal sealed class SwitchableToggleEditor : ToggleEditor
    {
        private EditorStateControls _editorStateControls;

        protected override void OnEnable()
        {
            base.OnEnable();

            _editorStateControls = new EditorStateControls(target, serializedObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorVisualControls.LabelField("Custom Settings");

            _editorStateControls.PropertyField(nameof(SwitchableToggle.CheckedObject));
            _editorStateControls.PropertyField(nameof(SwitchableToggle.UncheckedObject));

            serializedObject.ApplyModifiedProperties();
        }
    }
}