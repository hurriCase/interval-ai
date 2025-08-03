using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.Categories
{
    internal sealed class CategoriesRepository : ICategoriesRepository, IRepository
    {
        private readonly DefaultCategoriesConfig _defaultCategoriesConfig;
        private readonly DefaultWordsConfig _defaultWordsConfig;
        private readonly IIdHandler<CategoryEntry> _idHandler;

        //TODO:<Dmitriy.Sukharev> make this immutable
        public PersistentReactiveProperty<Dictionary<int, CategoryEntry>> CategoryEntries { get; } = new();

        internal CategoriesRepository(
            DefaultCategoriesConfig defaultCategoriesConfig,
            DefaultWordsConfig defaultWordsConfig,
            IIdHandler<CategoryEntry> idHandler)
        {
            _defaultCategoriesConfig = defaultCategoriesConfig;
            _defaultWordsConfig = defaultWordsConfig;
            _idHandler = idHandler;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                _idHandler.InitAsync(cancellationToken),

                CategoryEntries.InitAsync(PersistentKeys.CategoryEntriesKey, cancellationToken,
                    _idHandler.GenerateWithDefaultIds(_defaultCategoriesConfig.Defaults))
            };

            await UniTask.WhenAll(initTasks);

            foreach (var wordEntry in _defaultWordsConfig.Defaults)
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