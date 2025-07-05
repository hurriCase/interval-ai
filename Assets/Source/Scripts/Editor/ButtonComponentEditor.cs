using CustomUtils.Editor.CustomEditorUtilities;
using Source.Scripts.UI.CustomButton;
using UnityEditor;
using UnityEditor.UI;

namespace Source.Scripts.Editor
{
    [CustomEditor(typeof(ButtonComponent))]
    internal sealed class ButtonComponentEditor : ButtonEditor
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

            _editorStateControls.PropertyField(nameof(ButtonComponent.ButtonStateColorMapping));

            serializedObject.ApplyModifiedProperties();
        }
    }
}