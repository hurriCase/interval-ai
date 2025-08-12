using System.Collections.Generic;
using System.Threading;
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
    internal sealed class CategoriesRepository : ICategoriesRepository, IRepository
    {
        public ReadOnlyReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries => _categoryEntries.Property;
        private readonly PersistentReactiveProperty<Dictionary<int, CategoryEntry>> _categoryEntries = new();

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
                if (_categoryEntries.Value.TryGetValue(wordEntry.CategoryId, out var categoryEntry))
                    categoryEntry.WordEntries.Add(wordEntry);
            }
        }

        public CategoryEntry CreateCategory(string name)
        {
            var category = _categoryStateMutator.CreateCategoryEntry(name);
            _idHandler.AddEntry(category, _categoryEntries.Value);
            return category;
        }

        public void Dispose()
        {
            CategoryEntries.Dispose();
        }
    }
}