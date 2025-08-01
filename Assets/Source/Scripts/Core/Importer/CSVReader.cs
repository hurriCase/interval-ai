using System;
using System.Collections.Generic;
using Cysharp.Text;
using Source.Scripts.Core.Importer.Base;
using Source.Scripts.Core.Importer.CSVEntry;
using ZLinq;

namespace Source.Scripts.Core.Importer
{
    internal sealed class CSVReader : ICSVReader
    {
        private const char Quote = '"';
        private const char Comma = ',';

        public CSVTable Parse(string csvContent)
        {
            if (TryGetLines(csvContent, out var lines) is false)
                return new CSVTable(new CSVRow(Array.Empty<string>()), Array.Empty<CSVRow>());

            // Parse header
            var headerValues = ParseLine(lines[0]);
            var header = new CSVRow(headerValues);

            var rows = ParseRows(lines);

            return new CSVTable(header, rows);
        }

        private bool TryGetLines(string csvContent, out string[] lines)
        {
            lines = null;
            if (string.IsNullOrWhiteSpace(csvContent))
                return false;

            lines = csvContent.Split('\n')
                .AsValueEnumerable()
                .Where(line => string.IsNullOrWhiteSpace(line) is false)
                .Select(line => line.Trim())
                .ToArray();

            return lines.Length > 1;
        }

        private CSVRow[] ParseRows(IReadOnlyList<string> lines)
        {
            // skip the first header row
            var rows = new CSVRow[lines.Count - 1];
            for (var i = 1; i < lines.Count; i++)
            {
                var rowValues = ParseLine(lines[i]);
                rows[i - 1] = new CSVRow(rowValues);
            }

            return rows;
        }

        private string[] ParseLine(string line)
        {
            var values = new List<string>();
            var inQuotes = false;

            using var valueBuilder = ZString.CreateStringBuilder(false);

            foreach (var character in line)
            {
                switch (character)
                {
                    case Quote:
                        inQuotes = inQuotes is false;
                        break;

                    case Comma when inQuotes is false:
                        values.Add(valueBuilder.ToString().Trim());
                        valueBuilder.Clear();
                        break;

                    default:
                        valueBuilder.Append(character);
                        break;
                }
            }

            values.Add(valueBuilder.ToString().Trim());

            return values.ToArray();
        }
    }
}