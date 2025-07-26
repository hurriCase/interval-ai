using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Categories.Entries;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        public PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }

        internal CategoriesRepository(IDefaultCategoriesDatabase defaultCategoriesDatabase)
        {
            CategoryEntries =
                new PersistentReactiveProperty<List<CategoryEntry>>(PersistentPropertyKeys.CategoryEntryKey,
                    defaultCategoriesDatabase.Categories);
        }

        public void Dispose()
        {
            CategoryEntries?.Dispose();
        }
    }
}