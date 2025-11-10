using System;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Unsafe;
using R3;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using UnityEngine.AddressableAssets;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Categories.Category
{
    internal sealed partial class CategoryEntry
    {
        [Preserve]
        internal sealed class CategoryStateMutator : ICategoryStateMutator
        {
            public Observable<CategoryEntry> OnCategoryNameChanged => _categoryNameChanged;
            private readonly Subject<CategoryEntry> _categoryNameChanged = new();

            private readonly IIdHandler<CategoryEntry> _idHandler;
            private readonly IWordStateMutator _wordStateMutator;

            [Preserve]
            internal CategoryStateMutator(IIdHandler<CategoryEntry> idHandler, IWordStateMutator wordStateMutator)
            {
                _idHandler = idHandler;
                _wordStateMutator = wordStateMutator;
            }

            public CategoryEntry CreateCategoryEntry(string name, AssetReferenceSprite icon) =>
                new()
                {
                    Id = _idHandler.GetId(),
                    Name = name,
                    CategoryType = CategoryType.Created,
                    Icon = new CachedSprite(icon)
                };

            public void ChangeCategoryName(CategoryEntry categoryEntry, string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return;

                categoryEntry.Name = name;
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

                if (categoryEntry.WordEntries.Count == 0)
                    return;

                var sortRule = orderType switch
                {
                    WordOrderType.Default => static (x, y) => y.CreationData.CompareTo(x.CreationData),
                    WordOrderType.NewlyAdded => static (x, y) => y.CreationData.CompareTo(x.CreationData),
                    WordOrderType.OldlyAdded => static (x, y) => x.CreationData.CompareTo(y.CreationData),

                    WordOrderType.ByLearningState => static (x, y)
                        => UnsafeEnumConverter<LearningState>.ToInt32(x.LearningState)
                            .CompareTo(UnsafeEnumConverter<LearningState>.ToInt32(y.LearningState)),

                    WordOrderType.Alphabetically => static (x, y)
                        => string.Compare(x.Word.Learning, y.Word.Learning, StringComparison.OrdinalIgnoreCase),

                    WordOrderType.ReviewCount => (Comparison<WordEntry>)(static (x, y)
                        => y.ReviewCount.CompareTo(x.ReviewCount)),

                    _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
                };

                categoryEntry.WordEntries.Sort(sortRule);
            }
        }
    }
}