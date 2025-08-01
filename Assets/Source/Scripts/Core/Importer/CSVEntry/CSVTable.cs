namespace Source.Scripts.Core.Importer.CSVEntry
{
    internal readonly struct CSVTable
    {
        internal CSVRow Header { get; }
        internal CSVRow[] Rows { get; }

        internal CSVTable(CSVRow header, CSVRow[] rows)
        {
            Header = header;
            Rows = rows;
        }
    }
}