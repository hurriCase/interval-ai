using System;
using System.IO;
using CustomUtils.Editor.CustomEditorUtilities;
using CustomUtils.Editor.SheetsDownloader;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Importer;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEditor;
using UnityEngine;

namespace Source.Scripts.Editor.DefaultDataCreation
{
    internal sealed class GameDataWindow : SheetsDownloaderWindowBase<DefaultDataDatabase, DefaultDataSheet>
    {
        private Vector2 _scrollPosition;
        private CSVToBinaryConverter _csvToBinaryConverter;

        protected override DefaultDataDatabase Database => DefaultDataDatabase.Instance;

        [MenuItem("--Project--/" + nameof(GameDataWindow))]
        internal static void ShowWindow()
        {
            GetWindow<GameDataWindow>(nameof(GameDataWindow).ToSpacedWords());
        }

        protected override void InitializeWindow()
        {
            base.InitializeWindow();

            _csvToBinaryConverter = new CSVToBinaryConverter(new CSVReader(), new CSVMapper());
        }

        protected override void DrawCustomContent()
        {
            using var scrollScope = EditorVisualControls.CreateScrollView(ref _scrollPosition);

            DrawCommonSheetsSection();

            EditorGUILayout.Space();

            DrawConversionSection();
        }

        private void DrawConversionSection()
        {
            EditorVisualControls.H1Label("Binary Conversion");

            if (EditorVisualControls.Button("Convert All to Binary"))
                ConvertAllSheetsToBinary();
        }

        private void ConvertAllSheetsToBinary()
        {
            if (Database.Sheets == null || Database.Sheets.Count == 0)
            {
                EditorUtility.DisplayDialog("Warning", "No sheets available. Download sheets first.", "OK");
                return;
            }

            var convertedCount = 0;

            EnsureFolders(DefaultDataDatabase.BinaryPath);
            EnsureFolders(Database.GetDownloadPath());

            foreach (var sheet in Database.Sheets)
            {
                if (!sheet?.TextAsset)
                    continue;

                try
                {
                    ConvertSingleSheet(sheet);
                    convertedCount++;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to convert {sheet.Name}: {ex.Message}");
                }
            }

            AssetDatabase.Refresh();

            var message = convertedCount > 0
                ? $"Successfully converted {convertedCount} sheets to binary format!"
                : "No sheets were converted.";

            Debug.Log($"[GameDataWindow::ConvertAllSheetsToBinary] {message}");
        }

        private void EnsureFolders(string path)
        {
            if (Directory.Exists(path) is false)
                Directory.CreateDirectory(path);
        }

        private void ConvertSingleSheet(DefaultDataSheet sheet)
        {
            var csvPath = Path.Combine(Database.GetDownloadPath(), sheet.Name + ".csv");
            var binaryPath = Path.Combine(DefaultDataDatabase.BinaryPath, sheet.Name + ".bytes");

            switch (sheet.DefaultDataType)
            {
                case DefaultDataType.Words:
                    _csvToBinaryConverter.ConvertCSVToBinary<WordEntry>(csvPath, binaryPath);
                    break;

                case DefaultDataType.Categories:
                    _csvToBinaryConverter.ConvertCSVToBinary<CategoryEntry>(csvPath, binaryPath);
                    break;

                case DefaultDataType.None:
                    Debug.LogWarning("[GameDataWindow::ConvertSingleSheet] " +
                                     $"Unknown sheet type for {sheet.Name}. Skipping conversion.");
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log("[GameDataWindow::ConvertSingleSheet] " +
                      $"Converted {sheet.Name} to binary: {Path.GetFileName(DefaultDataDatabase.BinaryPath)}");
        }
    }
}