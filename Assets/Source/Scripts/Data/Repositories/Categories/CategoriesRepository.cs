using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        //TODO:<Dmitriy.Sukharev> make this immutable
        public PersistentReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries { get; }

        internal CategoriesRepository(
            DefaultCategoriesConfig defaultCategoriesConfig,
            DefaultWordsConfig defaultWordsConfig,
            IIdHandler<CategoryEntry> idHandler)
        {
            CategoryEntries =
                new PersistentReactiveProperty<Dictionary<int, CategoryEntry>>(PersistentKeys.CategoryEntriesKey,
                    idHandler.GenerateWithDefaultIds(defaultCategoriesConfig.Defaults));

            foreach (var wordEntry in defaultWordsConfig.Defaults)
            {
                if (CategoryEntries.Value.TryGetValue(wordEntry.CategoryId, out var categoryEntry))
                    categoryEntry.WordEntries.Add(wordEntry);
            }
        }

        public void Dispose()
        {
            CategoryEntries.Dispose();
        }
    }
}