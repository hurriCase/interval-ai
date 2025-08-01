using Source.Scripts.Core.Importer.CSVEntry;

namespace Source.Scripts.Core.Importer.Base
{
    internal interface ICSVMapper
    {
        T[] MapToObjects<T>(CSVTable csvTable) where T : new();
    }
}