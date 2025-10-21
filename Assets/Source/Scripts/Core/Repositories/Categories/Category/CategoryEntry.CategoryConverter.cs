using System;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.CSV.Base;
using CustomUtils.Runtime.CSV.CSVEntry;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Categories.Base;
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
            private const string CategoryTypeName = "CategoryType";
            private const string IconName = "Icon";

            protected override CategoryEntry ConvertRow(CsvRow row)
            {
                if (Enum.TryParse(row.GetValue(CategoryTypeName), out CategoryType categoryType) is false)
                    categoryType = CategoryType.Created;

                return new CategoryEntry
                {
                    Id = row.GetValue(IdName).ToInt(),
                    Name = row.GetValue(CategoryName),
                    CategoryType = categoryType,
                    Icon = CachedSprite.FromPath(row.GetValue(IconName))
                };
            }
        }
    }
}