using System.Collections.Generic;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Core.Repositories.Categories.Base
{
    internal interface ICategoriesRepository
    {
        PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }
    }
}