using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Categories;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        public PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }

        internal CategoriesRepository() =>
            CategoryEntries =
                new PersistentReactiveProperty<List<CategoryEntry>>(PersistentPropertyKeys.CategoryEntriesKey);

        public void Dispose()
        {
            CategoryEntries.Dispose();
        }
    }
}