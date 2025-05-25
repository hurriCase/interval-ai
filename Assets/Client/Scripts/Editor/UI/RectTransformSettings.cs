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

        [SerializeField] private float _parentWidth;
        [SerializeField] private float _leftMarginWidth;
        [SerializeField] private float _rightMarginWidth;
        [SerializeField] private float _contentWidth;

        [SerializeField] private float _parentHeight;
        [SerializeField] private float _topMarginHeight;
        [SerializeField] private float _bottomMarginHeight;
        [SerializeField] private float _contentHeight;

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
            _parentWidth = EditorGUIExtensions.FloatField("Parent Width", _parentWidth);
            _leftMarginWidth = EditorGUIExtensions.FloatField("Left Margin Width", _leftMarginWidth);
            _rightMarginWidth = EditorGUIExtensions.FloatField("Right Margin Width", _rightMarginWidth);
            _contentWidth = EditorGUIExtensions.FloatField("Content Width", _contentWidth);
        }

        private void DrawHeightSection()
        {
            _parentHeight = EditorGUIExtensions.FloatField("Parent Height", _parentHeight);
            _topMarginHeight = EditorGUIExtensions.FloatField("Top Margin Height", _topMarginHeight);
            _bottomMarginHeight = EditorGUIExtensions.FloatField("Bottom Margin Height", _bottomMarginHeight);
            _contentHeight = EditorGUIExtensions.FloatField("Content Height", _contentHeight);
        }
    }
}