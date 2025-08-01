using Source.Scripts.Core.Importer.CSVEntry;

namespace Source.Scripts.Core.Importer.Base
{
    internal interface ICSVReader
    {
        CSVTable Parse(string csvContent);
    }
}