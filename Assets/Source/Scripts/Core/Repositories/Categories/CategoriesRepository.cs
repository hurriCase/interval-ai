using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Base.Id;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words;

namespace Source.Scripts.Core.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IRepository
    {
        private readonly DefaultCategoriesDatabase _defaultCategoriesDatabase;
        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IIdHandler<CategoryEntry> _idHandler;

        //TODO:<Dmitriy.Sukharev> make this immutable
        public PersistentReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries { get; } = new();

        internal CategoriesRepository(
            DefaultCategoriesDatabase defaultCategoriesDatabase,
            DefaultWordsDatabase defaultWordsDatabase,
            IIdHandler<CategoryEntry> idHandler)
        {
            _defaultCategoriesDatabase = defaultCategoriesDatabase;
            _defaultWordsDatabase = defaultWordsDatabase;
            _idHandler = idHandler;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(cancellationToken),

                CategoryEntries.InitAsync(PersistentKeys.CategoryEntriesKey, cancellationToken,
                    _idHandler.GenerateWithDefaultIds(_defaultCategoriesDatabase.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            foreach (var wordEntry in _defaultWordsDatabase.Defaults)
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