using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
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

        public Observable<CategoryEntry> OnCategoryAdded => _categoryAdded;
        public Observable<CategoryEntry> OnCategoryRemoved => _categoryRemoved;

        private readonly Subject<CategoryEntry> _categoryAdded = new();
        private readonly Subject<CategoryEntry> _categoryRemoved = new();

        private readonly Dictionary<int, CategoryEntry> _unselectedCategories = new();

        private readonly DefaultCategoriesDatabase _defaultCategoriesDatabase;
        private readonly ICategoryStateMutator _categoryStateMutator;
        private readonly DefaultWordsDatabase _defaultWordsDatabase;
        private readonly IIdHandler<CategoryEntry> _idHandler;
        private readonly IAppConfig _appConfig;

        internal CategoriesRepository(
            DefaultCategoriesDatabase defaultCategoriesDatabase,
            ICategoryStateMutator categoryStateMutator,
            DefaultWordsDatabase defaultWordsDatabase,
            IIdHandler<CategoryEntry> idHandler,
            IAppConfig appConfig)
        {
            _defaultCategoriesDatabase = defaultCategoriesDatabase;
            _categoryStateMutator = categoryStateMutator;
            _defaultWordsDatabase = defaultWordsDatabase;
            _idHandler = idHandler;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            await _idHandler.InitAsync(token);
            await _categoryEntries.InitAsync(PersistentKeys.CategoryEntriesKey, token,
                _idHandler.GenerateWithDefaultIds(_defaultCategoriesDatabase.Defaults));

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

        public bool TrySelectRandomCategory()
        {
            foreach (var (_, categoryEntry) in _categoryEntries.Value)
            {
                if (categoryEntry.IsSelected && HasWordsToLearn(categoryEntry) is false)
                    continue;

                categoryEntry.IsSelected = true;
                _categoryEntries.SaveAsync();
                return true;
            }

            return false;
        }

        public Dictionary<int, CategoryEntry> GetUnselectedCategories()
        {
            _unselectedCategories.Clear();

            foreach (var (categoryId, categoryEntry) in _categoryEntries.Value)
            {
                if (categoryEntry.IsSelected is false)
                    _unselectedCategories[categoryId] = categoryEntry;
            }

            return _unselectedCategories;
        }

        public void SetSelectedCategories(List<int> categoryIds)
        {
            foreach (var categoryId in categoryIds)
            {
                if (_categoryEntries.Value.TryGetValue(categoryId, out var category))
                    category.IsSelected = true;
            }
        }

        [MustUseReturnValue]
        public string GetCategoryName(int categoryId) =>
            _categoryEntries.Value.TryGetValue(categoryId, out var categoryEntry)
                ? categoryEntry.LocalizationKey.GetLocalization()
                : string.Empty;

        private bool HasWordsToLearn(CategoryEntry categoryEntry)
        {
            foreach (var wordEntry in categoryEntry.WordEntries)
            {
                var isTargetState = _appConfig.IsTargetLearningState(PracticeState.NewWords, wordEntry.LearningState);

                if (wordEntry.IsHidden is false && isTargetState)
                    return true;
            }

            return false;
        }

        public void Dispose()
        {
            _categoryEntries.Dispose();
            _categoryAdded.Dispose();
            _categoryRemoved.Dispose();
        }
    }
}