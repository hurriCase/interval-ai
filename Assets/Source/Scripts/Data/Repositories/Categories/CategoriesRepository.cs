using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Categories.Base;
using UnityEngine.Scripting;
using CategoryEntry = Source.Scripts.Data.Repositories.Categories.Entries.CategoryEntry;

namespace Source.Scripts.Data.Repositories.Categories
{
    [Preserve]
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        public PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }

        [Preserve]
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