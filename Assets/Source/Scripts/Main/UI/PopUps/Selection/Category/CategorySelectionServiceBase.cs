using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Categories.Category;

namespace Source.Scripts.Main.UI.PopUps.Selection.Category
{
    internal abstract class CategorySelectionServiceBase : ISelectionService<int>, IDisposable
    {
        protected readonly ICategoriesRepository categoriesRepository;

        public IReadOnlyList<int> SelectionValues { get; protected set; }
        public bool IsSingleSelection => false;

        protected readonly ReactiveProperty<List<int>> selectedValues = new();
        protected Dictionary<int, CategoryEntry> currentCategories;

        private readonly string _selectionTitle;

        private readonly IDisposable _disposable;

        protected CategorySelectionServiceBase(
            ICategoriesRepository categoriesRepository,
            ILocalizationKeysDatabase localizationKeysDatabase)
        {
            this.categoriesRepository = categoriesRepository;
            _selectionTitle = localizationKeysDatabase.GetLocalization(LocalizationType.CategorySelectionName);

            _disposable = selectedValues
                .Where(values => values is { Count: > 0 })
                .Subscribe(this, static (selectedValues, self) => self.SetCategories(selectedValues));
        }

        internal abstract void UpdateData();
        protected abstract void SetCategories(List<int> selectedValues);

        public string GetSelectionName(int categoryId)
            => currentCategories[categoryId].LocalizationKey.GetLocalization();

        public string GetSelectionTitle() => _selectionTitle;

        public void SetValue(int categoryId, bool isSelected)
        {
            if (isSelected)
            {
                if (selectedValues.Value.Contains(categoryId) is false)
                    selectedValues.Value.Add(categoryId);
            }
            else
                selectedValues.Value.Remove(categoryId);

            selectedValues.OnNext(selectedValues.Value);
        }

        public bool GetSelectionState(int categoryId) => selectedValues.Value.Contains(categoryId);

        public void Dispose()
        {
            _disposable.Dispose();
            selectedValues.Dispose();
        }
    }
}