using CustomUtils.Runtime.CSV.Base;
using CustomUtils.Runtime.CSV.CSVEntry;
using CustomUtils.Runtime.Extensions;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        [Preserve]
        internal sealed class CategoryConverter : CsvConverterBase<CategoryEntry>
        {
            private const string IdName = "Id";
            private const string CategoryName = "Name";

            protected override CategoryEntry ConvertRow(CsvRow row) =>
                new()
                {
                    Id = row.GetValue(IdName).ToInt(),
                    Name = row.GetValue(CategoryName)
                };
        }
    }
}