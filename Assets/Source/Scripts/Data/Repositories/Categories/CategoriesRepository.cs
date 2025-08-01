using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        public PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; }

        internal CategoriesRepository(DefaultCategoriesConfig defaultCategoriesConfig)
        {
            CategoryEntries =
                new PersistentReactiveProperty<List<CategoryEntry>>(PersistentPropertyKeys.CategoryEntriesKey,
                    defaultCategoriesConfig.Defaults);
        }

        public void Dispose()
        {
            CategoryEntries.Dispose();
        }
    }
}