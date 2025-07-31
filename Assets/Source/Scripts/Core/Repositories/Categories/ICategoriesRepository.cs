using System.Collections.Generic;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Core.DI.Repositories.Categories
{
    internal interface ICategoriesRepository
    {
        PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }
    }
}