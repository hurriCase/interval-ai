using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using CategoryEntry = Source.Scripts.Data.Repositories.Categories.Entries.CategoryEntry;

namespace Source.Scripts.Data.Repositories.Categories.Base
{
    internal interface ICategoriesRepository
    {
        PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }
    }
}