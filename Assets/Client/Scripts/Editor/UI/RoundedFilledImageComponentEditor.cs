using Client.Scripts.UI.ProgressComponent;
using CustomUtils.Editor.Extensions;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace Client.Scripts.Editor.UI
{
    [CustomEditor(typeof(RoundedFilledImageComponent), true)]
    [CanEditMultipleObjects]
    internal sealed class RoundedFilledImageComponentEditor : ImageEditor
    {
        private SerializedProperty _roundedCapsProperty;
        private SerializedProperty _roundedCapResolutionProperty;
        private SerializedProperty _customFillOriginProperty;
        private SerializedProperty _useCustomFillOriginProperty;
        private SerializedProperty _thicknessRatioProperty;
        private SerializedProperty _typeProperty;
        private SerializedProperty _fillMethodProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            _roundedCapsProperty = serializedObject.FindField(nameof(RoundedFilledImageComponent.RoundedCaps));
            _roundedCapResolutionProperty =
                serializedObject.FindField(nameof(RoundedFilledImageComponent.RoundedCapResolution));
            _customFillOriginProperty =
                serializedObject.FindField(nameof(RoundedFilledImageComponent.CustomFillOrigin));
            _useCustomFillOriginProperty =
                serializedObject.FindField(nameof(RoundedFilledImageComponent.UseCustomFillOrigin));
            _thicknessRatioProperty = serializedObject.FindField(nameof(RoundedFilledImageComponent.ThicknessRatio));
            _typeProperty = serializedObject.FindProperty("m_Type");
            _fillMethodProperty = serializedObject.FindProperty("m_FillMethod");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rounded Fill Settings", EditorStyles.boldLabel);

            var isRadial360 = _typeProperty.enumValueIndex == (int)Image.Type.Filled &&
                              _fillMethodProperty.enumValueIndex == (int)Image.FillMethod.Radial360;

            using (new EditorGUI.DisabledScope(isRadial360 is false))
            {
                EditorGUILayout.PropertyField(_roundedCapsProperty);

                using (new EditorGUI.DisabledScope(!_roundedCapsProperty.boolValue))
                {
                    EditorGUILayout.PropertyField(_roundedCapResolutionProperty);
                }

                EditorGUILayout.PropertyField(_useCustomFillOriginProperty);

                using (new EditorGUI.DisabledScope(!_useCustomFillOriginProperty.boolValue))
                {
                    EditorGUILayout.PropertyField(_customFillOriginProperty);
                }

                EditorGUILayout.PropertyField(_thicknessRatioProperty);

                if (isRadial360 is false && _roundedCapsProperty.boolValue)
                    EditorGUILayout.HelpBox("Rounded caps only work with Radial 360 fill method.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}