using System.IO;
using MemoryPack;
using Source.Scripts.Core.Importer.Base;
using UnityEngine;

namespace Source.Scripts.Core.Importer
{
    internal sealed class CSVToBinaryConverter
    {
        private readonly ICSVReader _csvReader;
        private readonly ICSVMapper _csvMapper;

        internal CSVToBinaryConverter(ICSVReader csvReader, ICSVMapper csvMapper)
        {
            _csvReader = csvReader;
            _csvMapper = csvMapper;
        }

        public void ConvertCSVToBinary<T>(string csvFilePath, string binaryOutputPath) where T : new()
        {
            var csvContent = File.ReadAllText(csvFilePath);
            var csvTable = _csvReader.Parse(csvContent);
            var objects = _csvMapper.MapToObjects<T>(csvTable);

            var binaryData = MemoryPackSerializer.Serialize(objects);
            File.WriteAllBytes(binaryOutputPath, binaryData);

            Debug.Log($"Converted {objects.Length} objects to binary: {binaryOutputPath}");
        }
    }
}