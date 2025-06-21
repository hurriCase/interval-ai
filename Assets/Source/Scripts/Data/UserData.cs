using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Entries;
using Source.Scripts.Data.Entries.Words;

namespace Source.Scripts.Data
{
    internal class UserData : Singleton<UserData>
    {
        internal PersistentReactiveProperty<ProgressEntry> ProgressEntry { get; } =
            new(PersistentPropertyKeys.ProgressEntryKey, new ProgressEntry
                {
                    DailyWordGoal = 10
                }
            );

        internal PersistentReactiveProperty<List<WordEntry>> WordEntries { get; } =
            new(PersistentPropertyKeys.WordEntryKey);

        internal PersistentReactiveProperty<List<CategoryEntry>> CategoryEntries { get; } =
            new(PersistentPropertyKeys.CategoryEntryKey, DefaultCategoriesDatabase.Instance.Categories);
    }
}