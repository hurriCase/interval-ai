using CustomUtils.Editor.CustomEditorUtilities;
using Source.Scripts.UI.Components;
using TMPro.EditorUtilities;
using UnityEditor;

namespace Source.Scripts.Editor
{
    [CustomEditor(typeof(InputFieldComponent), true)]
    [CanEditMultipleObjects]
    internal sealed class InputFieldComponentEditor : TMP_InputFieldEditor
    {
        private EditorStateControls _editorStateControls;

        protected override void OnEnable()
        {
            base.OnEnable();

            _editorStateControls = new EditorStateControls(target, serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorVisualControls.LabelField("Custom Settings");

            _editorStateControls.PropertyField(nameof(InputFieldComponent.EditButton));

            serializedObject.ApplyModifiedProperties();
        }
    }
}