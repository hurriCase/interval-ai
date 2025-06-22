using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Entries;
using Source.Scripts.Data.Repositories.Entries.Words;

namespace Source.Scripts.Data.Repositories
{
    internal sealed class VocabularyRepository : Singleton<VocabularyRepository>
    {
        internal PersistentReactiveProperty<List<WordEntry>> WordEntries { get; } =
            new(PersistentPropertyKeys.WordEntryKey);

        internal PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; } =
            new(PersistentPropertyKeys.CategoryEntryKey, DefaultCategoriesDatabase.Instance.Categories);
    }
}