using System;
using System.Collections.Generic;
using UnityEditor;

namespace Client.Scripts.Editor.EditorCustomization
{
    /// <summary>
    /// Base class for all custom editors with enhanced GUI capabilities
    /// </summary>
    internal abstract class EditorBase : UnityEditor.Editor
    {
        /// <summary>
        /// Access to the enhanced GUI system with automatic undo support
        /// </summary>
        protected EditorGUIExtensions EditorGUIExtensions => _editorGUI ??= new EditorGUIExtensions(target);

        private EditorGUIExtensions _editorGUI;

        private const string PrefPrefix = "AbstractEditor_";
        private readonly Dictionary<string, bool> _foldoutStates = new();
        private bool _showDefaultInspector;

        protected virtual void OnEnable()
        {
            var targetTypeName = target.GetType().Name;
            _showDefaultInspector = EditorPrefs.GetBool($"{PrefPrefix}{targetTypeName}_DefaultInspector", false);

            _foldoutStates.Clear();
        }

        protected virtual void OnDisable()
        {
            var targetTypeName = target.GetType().Name;
            EditorPrefs.SetBool($"{PrefPrefix}{targetTypeName}_DefaultInspector", _showDefaultInspector);

            foreach (var entry in _foldoutStates)
                EditorPrefs.SetBool($"{PrefPrefix}{targetTypeName}_{SanitizeKey(entry.Key)}", entry.Value);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawCustomSections();

            DrawDefaultInspectorSection();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawCustomSections() { }

        private void DrawDefaultInspectorSection()
        {
            EditorGUILayoutExtensions.DrawBoxWithFoldout("Default Inspector", ref _showDefaultInspector,
                () => DrawDefaultInspector());
        }

        /// <summary>
        /// Draw a custom section with a foldout header.
        /// The foldout state is automatically saved between sessions.
        /// </summary>
        /// <param name="title">Title of the section</param>
        /// <param name="drawContent">Action to draw the section content</param>
        protected void DrawFoldoutSection(string title, Action drawContent)
        {
            _foldoutStates.TryAdd(title, true);

            var foldout = _foldoutStates[title];
            EditorGUILayoutExtensions.DrawBoxWithFoldout(title, ref foldout, drawContent);
            _foldoutStates[title] = foldout;
        }

        /// <summary>
        /// Draw a custom section without a foldout header.
        /// </summary>
        /// <param name="title">Title of the section</param>
        /// <param name="drawContent">Action to draw the section content</param>
        protected void DrawSection(string title, Action drawContent)
        {
            EditorGUILayoutExtensions.DrawBoxedSection(title, drawContent);
        }

        private string SanitizeKey(string key, bool reverse = false)
            => reverse ? key.Replace("_", " ").Replace("__", ".") : key.Replace(" ", "_").Replace(".", "__");
    }
}