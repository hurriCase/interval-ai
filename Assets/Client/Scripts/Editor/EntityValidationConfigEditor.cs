using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.Patterns.Attributes;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor
{
    [CustomEditor(typeof(EntityValidationConfig))]
    internal sealed class EntityValidationConfigEditor : InjectableEditor
    {
        [Inject] private IEntityValidationController _entityValidationController;

        private Dictionary<string, List<ValidationRule>> EntityRules => EntityValidationConfig.Instance.EntityRules;

        private readonly Dictionary<string, bool> _entityFoldouts = new();
        private Vector2 _scrollPosition;
        private bool _isLoading = true;
        private bool _isSaving;

        private List<Type> _entityTypes;

        protected override void OnEnable()
        {
            base.OnEnable();

            InitializeEntityTypes();
            InitializeCloudData();
        }

        private void InitializeEntityTypes()
        {
            _entityTypes = Assembly.GetAssembly(typeof(EntityValidationConfig))
                .GetTypes()
                .Where(type => type.GetProperties()
                    .Any(propertyInfo => propertyInfo.GetCustomAttribute<ValidationAttribute>() != null))
                .ToList();
        }

        private async void InitializeCloudData()
        {
            _isLoading = true;

            try
            {
                await _entityValidationController.LoadEntityValidationRulesAsync();

                EditorUtility.SetDirty(target);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"[EntityValidationConfigEditor::InitializeCloudData] Failed to load config: {e.Message}");
            }
            finally
            {
                _isLoading = false;
                Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            if (_isLoading)
            {
                EditorGUILayout.HelpBox("Loading validation rules...", MessageType.Info);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            DrawEntityList();
            DrawAddEntityButton();
            DrawSaveButton();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (_isSaving) EditorGUILayout.HelpBox("Saving validation rules...", MessageType.Info);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawEntityList()
        {
            foreach (var entityName in EntityRules.Keys.ToList())
            {
                if (_entityFoldouts.ContainsKey(entityName) is false)
                    _entityFoldouts[entityName] = false;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                DrawEntityHeader(entityName);

                if (_entityFoldouts[entityName])
                {
                    DrawEntityProperties(entityName);
                    DrawAddPropertyButton(entityName);
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawEntityHeader(string entityName)
        {
            EditorGUILayout.BeginHorizontal();

            // Add indentation before foldout
            EditorGUI.indentLevel++;
            _entityFoldouts[entityName] = EditorGUILayout.Foldout(
                _entityFoldouts[entityName],
                entityName,
                true
            );
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                EntityRules.Remove(entityName);
                return;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawEntityProperties(string entityName)
        {
            var rules = EntityRules[entityName];

            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;

                DrawPropertyField(entityName, rule);
                DrawValidationTypeField(rule);
                DrawValidationParameters(rule);

                if (GUILayout.Button("Remove Property", GUILayout.Width(120)))
                {
                    rules.RemoveAt(i);
                    break;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawPropertyField(string entityName, ValidationRule rule)
        {
            if (entityName == null) return;
            var entityType = _entityTypes.FirstOrDefault(t => t.Name == entityName);
            var properties = entityType
                ?.GetProperties()
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<ValidationAttribute>() != null)
                .Select(propertyInfo => propertyInfo.Name)
                .ToArray();

            if (properties == null) return;
            var currentIndex = Array.IndexOf(properties, rule.PropertyName);
            var newIndex = EditorGUILayout.Popup("Property", currentIndex, properties);

            if (newIndex != currentIndex && newIndex >= 0) rule.PropertyName = properties[newIndex];
        }

        private void DrawValidationTypeField(ValidationRule rule)
        {
            rule.ValidationType = (ValidationType)EditorGUILayout.EnumPopup(
                "Validation Type",
                rule.ValidationType
            );
        }

        private void DrawValidationParameters(ValidationRule rule)
        {
            switch (rule.ValidationType)
            {
                case ValidationType.StringLength:
                case ValidationType.NumericRange:
                    if (!rule.Parameters.ContainsKey("min"))
                        rule.Parameters["min"] = 0;
                    if (!rule.Parameters.ContainsKey("max"))
                        rule.Parameters["max"] = 100;

                    rule.Parameters["min"] = EditorGUILayout.FloatField(
                        "Minimum",
                        Convert.ToSingle(rule.Parameters["min"])
                    );
                    rule.Parameters["max"] = EditorGUILayout.FloatField(
                        "Maximum",
                        Convert.ToSingle(rule.Parameters["max"])
                    );
                    break;
            }
        }

        private void DrawAddEntityButton()
        {
            if (GUILayout.Button("Add Entity") is false)
                return;

            var menu = new GenericMenu();

            foreach (var entityType in _entityTypes)
            {
                if (EntityRules.ContainsKey(entityType.Name) is false)
                    menu.AddItem(
                        new GUIContent(entityType.Name),
                        false,
                        () => AddEntity(entityType.Name)
                    );
            }

            menu.ShowAsContext();
        }

        private void DrawAddPropertyButton(string entityName)
        {
            EditorGUILayout.Space(5);
            if (GUILayout.Button("Add Property")) EntityRules[entityName].Add(new ValidationRule());
        }

        private void DrawSaveButton()
        {
            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = !_isSaving;
            if (GUILayout.Button("Save to Cloud", GUILayout.Width(120), GUILayout.Height(30))) SaveToCloudAsync();

            GUI.enabled = true;

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private async void SaveToCloudAsync()
        {
            if (_isSaving) return;

            _isSaving = true;
            try
            {
                await _entityValidationController.SaveEntityValidationRulesAsync();

                Debug.Log("[EntityValidationConfigEditor::SaveToCloudAsync] Validation rules saved successfully!");
                EditorUtility.DisplayDialog("Success", "Validation rules saved successfully!", "OK");
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"[EntityValidationConfigEditor::SaveToCloudAsync] Failed to save validation rules: {e.Message}");
                EditorUtility.DisplayDialog("Error", "Failed to save validation rules. Check console for details.",
                    "OK");
            }
            finally
            {
                _isSaving = false;
                Repaint();
            }
        }

        private void AddEntity(string entityName)
        {
            EntityRules[entityName] = new List<ValidationRule>();
        }
    }
}