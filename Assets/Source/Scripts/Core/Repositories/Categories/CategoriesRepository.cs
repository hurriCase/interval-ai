using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;
using Source.Scripts.Core.Repositories.Words;

namespace Source.Scripts.Core.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IRepository, IDisposable
    {
        public ReadOnlyReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries => _categoryEntries.Property;
        private readonly PersistentReactiveProperty<Dictionary<int, CategoryEntry>> _categoryEntries = new();

        public Observable<CategoryEntry> CategoryAdded => _categoryAdded;
        public Observable<CategoryEntry> CategoryRemoved => _categoryRemoved;

        private readonly Subject<CategoryEntry> _categoryAdded = new();
        private readonly Subject<CategoryEntry> _categoryRemoved = new();

        private readonly DefaultCategoriesDatabase _defaultCategoriesDatabase;
        private readonly ICategoryStateMutator _categoryStateMutator;
        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IIdHandler<CategoryEntry> _idHandler;

        internal CategoriesRepository(
            DefaultCategoriesDatabase defaultCategoriesDatabase,
            ICategoryStateMutator categoryStateMutator,
            DefaultWordsDatabase defaultWordsDatabase,
            IIdHandler<CategoryEntry> idHandler)
        {
            _defaultCategoriesDatabase = defaultCategoriesDatabase;
            _categoryStateMutator = categoryStateMutator;
            _defaultWordsDatabase = defaultWordsDatabase;
            _idHandler = idHandler;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(cancellationToken),

                _categoryEntries.InitAsync(PersistentKeys.CategoryEntriesKey, cancellationToken,
                    _idHandler.GenerateWithDefaultIds(_defaultCategoriesDatabase.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            foreach (var wordEntry in _defaultWordsDatabase.Defaults)
            {
                foreach (var categoryId in wordEntry.CategoryIds)
                {
                    if (_categoryEntries.Value.TryGetValue(categoryId, out var categoryEntry))
                        categoryEntry.WordEntries.Add(wordEntry);
                }
            }
        }

        public CategoryEntry CreateCategory(string name)
        {
            var category = _categoryStateMutator.CreateCategoryEntry(name);

            _categoryEntries.Value[category.Id] = category;
            _categoryEntries.SaveAsync();

            _categoryAdded.OnNext(category);

            return category;
        }

        public void RemoveCategory(CategoryEntry categoryEntry)
        {
            _categoryEntries.Value.Remove(categoryEntry.Id);
            _categoryEntries.SaveAsync();

            _categoryRemoved.OnNext(categoryEntry);
        }

        public string GetCategoryName(int categoryId) =>
            _categoryEntries.Value.TryGetValue(categoryId, out var categoryEntry)
                ? categoryEntry.LocalizationKey.GetLocalization()
                : string.Empty;

        public void Dispose()
        {
            _categoryEntries.Dispose();
            _categoryAdded.Dispose();
            _categoryRemoved.Dispose();
        }
    }
}