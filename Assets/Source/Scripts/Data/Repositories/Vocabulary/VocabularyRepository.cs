using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal sealed class VocabularyRepository : Singleton<VocabularyRepository>
    {
        internal PersistentReactiveProperty<List<Entries.WordEntry>> WordEntries { get; } =
            new(PersistentPropertyKeys.WordEntryKey);

        internal PersistentReactiveProperty<List<Entries.CategoryEntry>> CategoryEntries { get; } =
            new(PersistentPropertyKeys.CategoryEntryKey, DefaultCategoriesDatabase.Instance.Categories);
    }
}