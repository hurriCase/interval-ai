using System;
using System.Collections.Generic;
using UnityEditor;

namespace Client.Scripts.Editor.EditorCustomization
{
    /// <summary>
    /// Abstract base editor class that provides common functionality for custom editors.
    /// Implements default inspector with state persistence and customizable UI sections.
    /// </summary>
    public abstract class EditorBase : UnityEditor.Editor
    {
        private const string PrefPrefix = "AbstractEditor_";

        private readonly Dictionary<string, bool> _foldoutStates = new();

        private bool _showDefaultInspector;

        /// <summary>
        /// Called when the editor is enabled to initialize properties and state.
        /// </summary>
        protected virtual void OnEnable()
        {
            // Load saved states from EditorPrefs
            LoadFoldoutStates();
        }

        /// <summary>
        /// Called when the editor is disabled to save state.
        /// </summary>
        protected virtual void OnDisable()
        {
            // Save states to EditorPrefs
            SaveFoldoutStates();
        }

        /// <summary>
        /// Draw the inspector GUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawCustomSections();

            DrawDefaultInspectorSection();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Override this to draw custom sections in the inspector.
        /// </summary>
        protected virtual void DrawCustomSections() { }

        /// <summary>
        /// Draws the default inspector in a foldout section.
        /// </summary>
        protected void DrawDefaultInspectorSection()
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

            if (_foldoutStates != null && _foldoutStates[title] != foldout)
                _foldoutStates[title] = foldout;
        }

        /// <summary>
        /// Load all foldout states from EditorPrefs
        /// </summary>
        private void LoadFoldoutStates()
        {
            var targetTypeName = target.GetType().Name;

            _showDefaultInspector = EditorPrefs.GetBool($"{PrefPrefix}{targetTypeName}_DefaultInspector", false);

            _foldoutStates.Clear();
        }

        /// <summary>
        /// Save all foldout states to EditorPrefs
        /// </summary>
        private void SaveFoldoutStates()
        {
            var targetTypeName = target.GetType().Name;

            EditorPrefs.SetBool($"{PrefPrefix}{targetTypeName}_DefaultInspector", _showDefaultInspector);

            foreach (var entry in _foldoutStates)
            {
                EditorPrefs.SetBool(
                    $"{PrefPrefix}{targetTypeName}_{SanitizeKey(entry.Key)}",
                    entry.Value);
            }
        }

        /// <summary>
        /// Get the saved foldout state for a specific section
        /// </summary>
        private bool GetFoldoutState(string sectionKey, bool defaultValue = true)
        {
            var targetTypeName = target.GetType().Name;
            return EditorPrefs.GetBool(
                $"{PrefPrefix}{targetTypeName}_{SanitizeKey(sectionKey)}",
                defaultValue);
        }

        /// <summary>
        /// Sanitize a key for use in EditorPrefs
        /// </summary>
        private string SanitizeKey(string key) => key.Replace(" ", "_").Replace(".", "_");
    }
}