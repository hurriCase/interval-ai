using System.Reflection;
using Client.Scripts.Editor.EditorCustomization;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor.UI
{
    [CustomEditor(typeof(RectTransform), true)]
    internal sealed class RectTransformExtendedEditor : EditorBase
    {
        private UnityEditor.Editor _defaultEditor;

        private float _parentWidth;
        private float _leftMarginWidth;
        private float _rightMarginWidth;
        private float _contentWidth;

        private float _parentHeight;
        private float _topMarginHeight;
        private float _bottomMarginHeight;
        private float _contentHeight;

        protected override void OnEnable()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var rectTransformEditorType = assembly.GetType("UnityEditor.RectTransformEditor");

            _defaultEditor = CreateEditor(targets, rectTransformEditorType);
        }

        protected override void OnDisable()
        {
            if (_defaultEditor)
            {
                DestroyImmediate(_defaultEditor);
                _defaultEditor = null;
            }

            base.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            _defaultEditor.OnInspectorGUI();

            serializedObject.Update();

            DrawCustomSections();

            serializedObject.ApplyModifiedProperties();
        }

        protected override void DrawCustomSections()
        {
            DrawFoldoutSection("Width", DrawWidthSection);
            DrawFoldoutSection("Height", DrawHeightSection);

            if (GUILayout.Button("Apply anchors") is false)
                return;

            foreach (var rectTransformTarget in targets)
            {
                var rectTransform = (RectTransform)rectTransformTarget;

                var widthAnchorRatio = 1f / _parentWidth;
                var heightAnchorRatio = 1f / _parentHeight;

                var leftMarginWidthAnchor = widthAnchorRatio * _leftMarginWidth;
                var rightMarginWidthAnchor = widthAnchorRatio * _rightMarginWidth;

                var topMarginHeightAnchor = heightAnchorRatio * _topMarginHeight;
                var bottomMarginHeightAnchor = heightAnchorRatio * _bottomMarginHeight;

                Undo.RecordObject(rectTransform, "Set RectTransform Anchors");

                rectTransform.anchorMin = new Vector2(leftMarginWidthAnchor, bottomMarginHeightAnchor);
                rectTransform.anchorMax = new Vector2(1 - rightMarginWidthAnchor, 1 - topMarginHeightAnchor);

                EditorUtility.SetDirty(rectTransform);
            }
        }

        private void DrawWidthSection()
        {
            _parentWidth = EditorGUILayout.FloatField("Parent Width", _parentWidth);
            _leftMarginWidth = EditorGUILayout.FloatField("Left Margin Width", _leftMarginWidth);
            _rightMarginWidth = EditorGUILayout.FloatField("Right Margin Width", _rightMarginWidth);
            _contentWidth = EditorGUILayout.FloatField("Content Width", _contentWidth);
        }

        private void DrawHeightSection()
        {
            _parentHeight = EditorGUILayout.FloatField("Parent Height", _parentHeight);
            _topMarginHeight = EditorGUILayout.FloatField("Top Margin Height", _topMarginHeight);
            _bottomMarginHeight = EditorGUILayout.FloatField("Bottom Margin Height", _bottomMarginHeight);
            _contentHeight = EditorGUILayout.FloatField("Content Height", _contentHeight);
        }
    }
}