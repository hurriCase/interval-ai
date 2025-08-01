using System;
using CustomUtils.Editor.CustomEditorUtilities;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Importer;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Words;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Editor
{
    internal sealed class DefaultEntriesCreatorWindow : WindowBase
    {
        private Object _inputCSVFile;
        private Object _outputBinaryFile;

        private DefaultType _defaultType;

        private CSVToBinaryConverter _csvToBinaryConverter;

        [MenuItem("--Project--/DefaultEntriesCreatorWindow")]
        internal static void ShowWindow()
        {
            GetWindow<DefaultEntriesCreatorWindow>(nameof(DefaultEntriesCreatorWindow).ToSpacedWords());
        }

        protected override void InitializeWindow()
        {
            _csvToBinaryConverter = new CSVToBinaryConverter(new CSVReader(), new CSVMapper());
        }

        protected override void DrawWindowContent()
        {
            _defaultType = EditorStateControls.EnumField(_defaultType);

            _inputCSVFile =
                EditorStateControls.ObjectField("CSV File", _inputCSVFile, typeof(TextAsset)) as TextAsset;

            _outputBinaryFile =
                EditorStateControls.ObjectField("Binary File", _outputBinaryFile, typeof(TextAsset)) as TextAsset;

            if (!_inputCSVFile)
            {
                EditorVisualControls.InfoBox("Assign Input CSV File first.");
                return;
            }

            EditorVisualControls.Button("Get binary representation of the CSV file", ConvertCSVToBinary);
        }

        private void ConvertCSVToBinary()
        {
            var inputPath = AssetDatabase.GetAssetPath(_inputCSVFile);

            if (inputPath.Contains(".csv") is false)
            {
                EditorVisualControls.WarningBox("File is not a CSV file.");
                return;
            }

            var outputPath = inputPath.Replace(".csv", ".bytes");

            switch (_defaultType)
            {
                case DefaultType.Words:
                    _csvToBinaryConverter.ConvertCSVToBinary<WordEntry>(inputPath, outputPath);
                    break;

                case DefaultType.Categories:
                    _csvToBinaryConverter.ConvertCSVToBinary<CategoryEntry>(inputPath, outputPath);
                    break;

                case DefaultType.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            AssetDatabase.Refresh();

            _outputBinaryFile = AssetDatabase.LoadAssetAtPath<TextAsset>(outputPath);
        }

        private enum DefaultType
        {
            None = 0,
            Words = 1,
            Categories = 2
        }
    }
}