using System.Collections.Generic;
using CategoryEntry = Source.Scripts.Data.Repositories.Categories.Entries.CategoryEntry;

namespace Source.Scripts.Data.Repositories.Categories.Base
{
    internal interface IDefaultCategoriesDatabase
    {
        List<CategoryEntry> Categories { get; }
    }
}