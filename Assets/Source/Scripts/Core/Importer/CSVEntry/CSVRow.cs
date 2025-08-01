namespace Source.Scripts.Core.Importer.CSVEntry
{
    internal readonly struct CSVRow
    {
        internal string[] Values { get; }

        internal CSVRow(string[] values)
        {
            Values = values;
        }
    }
}