using System;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        internal sealed class CategoryStateMutator : ICategoryStateMutator
        {
            public Observable<CategoryEntry> CategoryNameChanged => _categoryNameChanged;
            private readonly Subject<CategoryEntry> _categoryNameChanged = new();

            private readonly IIdHandler<CategoryEntry> _idHandler;
            private readonly IWordStateMutator _wordStateMutator;

            internal CategoryStateMutator(IIdHandler<CategoryEntry> idHandler, IWordStateMutator wordStateMutator)
            {
                _idHandler = idHandler;
                _wordStateMutator = wordStateMutator;
            }

            public CategoryEntry CreateCategoryEntry(string name) =>
                new()
                {
                    Id = _idHandler.GetId(),
                    LocalizationKey = name
                };

            public void ChangeCategoryName(CategoryEntry categoryEntry, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return;

                categoryEntry.LocalizationKey = name;
                _categoryNameChanged.OnNext(categoryEntry);
            }

            public void ChangeWordOrder(CategoryEntry categoryEntry, WordOrderType newOrderType)
            {
                categoryEntry.WordOrderType = newOrderType;
                SortWords(categoryEntry);
            }

            public void ResetWordsProgress(CategoryEntry categoryEntry)
            {
                foreach (var wordEntry in categoryEntry.WordEntries)
                    _wordStateMutator.ResetWord(wordEntry);
            }

            private void SortWords(CategoryEntry categoryEntry)
            {
                var orderType = categoryEntry.WordOrderType;

                if (categoryEntry.WordEntries.Count == 0 || orderType == WordOrderType.None)
                    return;

                var sortRule = orderType switch
                {
                    WordOrderType.Default => (x, y) => y.CreationData.CompareTo(x.CreationData),
                    WordOrderType.NewlyAdded => (x, y) => y.CreationData.CompareTo(x.CreationData),
                    WordOrderType.OldlyAdded => (x, y) => x.CreationData.CompareTo(y.CreationData),

                    WordOrderType.ByLearningState => (x, y)
                        => UnsafeEnumConverter<LearningState>.ToInt32(x.LearningState)
                            .CompareTo(UnsafeEnumConverter<LearningState>.ToInt32(y.LearningState)),

                    WordOrderType.Alphabetically => (x, y)
                        => string.Compare(x.LearningWord, y.LearningWord, StringComparison.OrdinalIgnoreCase),

                    WordOrderType.ReviewCount => (Comparison<WordEntry>)((x, y)
                        => y.ReviewCount.CompareTo(x.ReviewCount)),

                    _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
                };

                categoryEntry.WordEntries.Sort(sortRule);
            }
        }
    }
}