using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        //TODO:<Dmitriy.Sukharev> make this immutable
        public PersistentReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries { get; }

        internal CategoriesRepository(
            DefaultCategoriesConfig defaultCategoriesConfig,
            IIdHandler<CategoryEntry> idHandler)
        {
            CategoryEntries =
                new PersistentReactiveProperty<Dictionary<int, CategoryEntry>>(PersistentKeys.CategoryEntriesKey,
                    idHandler.GenerateWithDefaultIds(defaultCategoriesConfig.Defaults));
        }

        public void Dispose()
        {
            CategoryEntries.Dispose();
        }
    }
}