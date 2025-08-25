using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Repositories.Categories.Category;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class CategorySelectionService : SelectionService<int>
    {
        public ReactiveProperty<List<int>> SelectedValues { get; } = new();

        private readonly string _selectionTitle;
        private Dictionary<int, CategoryEntry> _selectionValues;

        public CategorySelectionService(string selectionTitle)
            : base(Array.Empty<int>(), false)
        {
            _selectionTitle = selectionTitle;
        }

        internal void UpdateData(
            Dictionary<int, CategoryEntry> selectionValues,
            List<int> selectedValues = null)
        {
            SelectionValues = selectionValues.Keys.ToArray();
            _selectionValues = selectionValues;
            SelectedValues.Value = selectedValues ?? new List<int>();
        }

        public override string GetSelectionName(int categoryId)
            => _selectionValues[categoryId].LocalizationKey.GetLocalization();

        public override string GetSelectionTitle() => _selectionTitle;

        public override void SetValue(int categoryId, bool isSelected)
        {
            if (isSelected)
            {
                if (SelectedValues.Value.Contains(categoryId) is false)
                    SelectedValues.Value.Add(categoryId);
            }
            else
                SelectedValues.Value.Remove(categoryId);

            SelectedValues.OnNext(SelectedValues.Value);
        }

        public override bool GetSelectionState(int categoryId) => SelectedValues.Value.Contains(categoryId);
    }
}