using System;
using Client.Scripts.Core.AI;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using CustomUtils.Runtime.Extensions;
using DependencyInjection.Editor;
using DependencyInjection.Runtime.InjectionBase;
using UnityEditor;
using UnityEngine;

namespace Client.Scripts.Editor
{
    internal sealed class AISettingsWindow : InjectableEditorWindow
    {
        [Inject] private ICloudRepository _repositoryController;

        private GenerativeModel _settings;
        private Vector2 _scrollPosition;
        private bool _showAdvancedSettings;
        private bool _isLoading = true;
        private bool _isSaving;

        private string[] _mimeTypeNames;
        private ResponseMimeType[] _mimeTypeValues;

        [MenuItem("Project/AI Settings", priority = 4)]
        public static void ShowWindow()
        {
            var window = GetWindow<AISettingsWindow>();
            window.titleContent = new GUIContent("AI Settings");
            window.minSize = new Vector2(400, 600);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            InitializeMimeTypes();
            InitializeSettings();
        }

        private void InitializeMimeTypes()
        {
            _mimeTypeValues = (ResponseMimeType[])Enum.GetValues(typeof(ResponseMimeType));
            _mimeTypeNames = new string[_mimeTypeValues.Length];

            for (var i = 0; i < _mimeTypeValues.Length; i++)
                _mimeTypeNames[i] = _mimeTypeValues[i].GetJsonPropertyName();
        }

        private async void InitializeSettings()
        {
            _isLoading = true;

            try
            {
                await _repositoryController.InitAsync();
                _settings =
                    await _repositoryController.ReadDataAsync<GenerativeModel>(DataType.Configs,
                        DBConfig.Instance.AIConfigPath)
                    ?? new GenerativeModel();
            }
            catch (Exception e)
            {
                Debug.LogError($"[AISettingsWindow::InitializeSettings] Failed to initialize settings: {e.Message}");
                _settings = new GenerativeModel();
            }
            finally
            {
                _isLoading = false;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_isLoading)
            {
                EditorGUILayout.HelpBox("Loading settings...", MessageType.Info);
                return;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.Space(10);
            DrawBasicSettings();

            EditorGUILayout.Space(20);
            DrawAdvancedSettings();

            EditorGUILayout.Space(20);
            DrawSaveButton();

            EditorGUILayout.EndScrollView();

            if (_isSaving)
                EditorGUILayout.HelpBox("Saving settings...", MessageType.Info);
        }

        private void DrawBasicSettings()
        {
            EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            using (new EditorGUI.ChangeCheckScope())
            {
                _settings.ApiKey = EditorGUILayout.TextField("API Key", _settings.ApiKey);
                _settings.ModelName = EditorGUILayout.TextField("Model Name", _settings.ModelName);
                _settings.EndpointFormat = EditorGUILayout.TextField("Endpoint Format", _settings.EndpointFormat);
            }

            EditorGUI.indentLevel--;
        }

        private void DrawAdvancedSettings()
        {
            _showAdvancedSettings = EditorGUILayout.Foldout(_showAdvancedSettings, "Advanced Settings", true);

            if (_showAdvancedSettings)
            {
                EditorGUI.indentLevel++;
                var config = _settings.GenerationConfig;

                using (new EditorGUI.ChangeCheckScope())
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField(
                        new GUIContent("Stop Sequences", "List of sequences where the model should stop generating"));

                    if (config.StopSequences == null)
                        config.StopSequences = Array.Empty<string>();

                    if (GUILayout.Button("Add Stop Sequence"))
                    {
                        var newList = new string[config.StopSequences.Length + 1];
                        Array.Copy(config.StopSequences, newList, config.StopSequences.Length);
                        newList[^1] = "";
                        config.StopSequences = newList;
                    }

                    for (var i = 0; i < config.StopSequences.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        config.StopSequences[i] =
                            EditorGUILayout.TextField($"Sequence {i + 1}", config.StopSequences[i]);

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            var newList = new string[config.StopSequences.Length - 1];
                            Array.Copy(config.StopSequences, 0, newList, 0, i);
                            Array.Copy(config.StopSequences, i + 1, newList, i, config.StopSequences.Length - i - 1);
                            config.StopSequences = newList;
                            GUIUtility.ExitGUI();
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    var currentMimeType = config.ResponseMimeType;
                    var currentIndex = Array.IndexOf(_mimeTypeValues, currentMimeType);
                    var newIndex = EditorGUILayout.Popup(
                        new GUIContent("Response MIME Type", "Specifies the format of the response"),
                        currentIndex, _mimeTypeNames);

                    if (newIndex != currentIndex) config.ResponseMimeType = _mimeTypeValues[newIndex];

                    config.CandidateCount = EditorGUILayout.IntField(
                        new GUIContent("Candidate Count", "Number of alternative responses to generate"),
                        config.CandidateCount);

                    config.MaxOutputTokens = EditorGUILayout.IntField(
                        new GUIContent("Max Output Tokens", "Maximum number of tokens to generate in the response"),
                        config.MaxOutputTokens);

                    config.Temperature = EditorGUILayout.Slider(
                        new GUIContent("Temperature", "Controls randomness (0.0 = focused, 1.0 = creative)"),
                        config.Temperature, 0f, 1f);

                    config.TopP = EditorGUILayout.Slider(
                        new GUIContent("Top P", "Probability threshold for token selection"),
                        config.TopP, 0f, 1f);

                    config.TopK = EditorGUILayout.IntField(
                        new GUIContent("Top K", "Limits the number of tokens considered for generation"),
                        config.TopK);

                    config.PresencePenalty = EditorGUILayout.Slider(
                        new GUIContent("Presence Penalty", "Penalizes tokens based on their presence"),
                        config.PresencePenalty, -2f, 2f);

                    config.FrequencyPenalty = EditorGUILayout.Slider(
                        new GUIContent("Frequency Penalty", "Penalizes tokens based on their frequency"),
                        config.FrequencyPenalty, -2f, 2f);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    config.ResponseLogprobs = EditorGUILayout.Toggle(
                        new GUIContent("Response Log Probs", "Include token probabilities in the response"),
                        config.ResponseLogprobs);

                    using (new EditorGUI.DisabledGroupScope(!config.ResponseLogprobs))
                    {
                        EditorGUI.BeginChangeCheck();
                        var logprobs = EditorGUILayout.IntField(
                            new GUIContent("Log Probs Count", "Number of most probable tokens to return per position"),
                            config.Logprobs ?? 3);
                        if (EditorGUI.EndChangeCheck() && config.ResponseLogprobs)
                            config.Logprobs = logprobs;
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUI.indentLevel--;
            }
        }

        private void DrawSaveButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = !_isSaving;
            if (GUILayout.Button("Save Settings", GUILayout.Width(120), GUILayout.Height(30)))
                SaveSettingsAsync();

            GUI.enabled = true;

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private async void SaveSettingsAsync()
        {
            if (_isSaving) return;

            _isSaving = true;
            try
            {
                await _repositoryController.WriteDataAsync(DataType.Configs, DBConfig.Instance.AIConfigPath, _settings);
                Debug.Log("[AISettingsWindow::SaveSettingsAsync] AI Settings saved successfully!");
                ShowNotification(new GUIContent("Settings saved successfully!"));
            }
            catch (Exception e)
            {
                Debug.LogError($"[AISettingsWindow::SaveSettingsAsync] Failed to save AI Settings: {e.Message}");
                EditorUtility.DisplayDialog("Error", "Failed to save settings. Check console for details.", "OK");
            }
            finally
            {
                _isSaving = false;
                Repaint();
            }
        }
    }
}