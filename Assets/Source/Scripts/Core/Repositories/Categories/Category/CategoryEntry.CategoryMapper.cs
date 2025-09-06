using CustomUtils.Runtime.CSV.Base;
using CustomUtils.Runtime.CSV.CSVEntry;
using CustomUtils.Runtime.Extensions;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        internal sealed class CategoryConverter : CsvConverterBase<CategoryEntry>
        {
            private const string IdName = "Id";
            private const string LocalizationKeyName = "LocalizationKey";

            protected override CategoryEntry ConvertRow(CsvRow row) =>
                new()
                {
                    Id = row.GetValue(IdName).ToInt(),
                    LocalizationKey = row.GetValue(LocalizationKeyName)
                };
        }
    }
}