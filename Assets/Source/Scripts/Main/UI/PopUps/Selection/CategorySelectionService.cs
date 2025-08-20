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

        private Dictionary<int, CategoryEntry> _selectionValues;

        public CategorySelectionService(string selectionKey)
            : base(Array.Empty<int>(), false, selectionKey) { }

        internal void UpdateData(
            Dictionary<int, CategoryEntry> selectionValues,
            List<int> selectedValues)
        {
            SelectionValues = selectionValues.Keys.ToArray();
            _selectionValues = selectionValues;
            SelectedValues.Value = selectedValues;
        }

        public override string GetSelectionName(int categoryId)
            => _selectionValues[categoryId].LocalizationKey.GetLocalization();

        public override void SetValue(int categoryId, bool isSelected)
        {
            if (isSelected)
            {
                if (SelectedValues.Value.Contains(categoryId) is false)
                    SelectedValues.Value.Add(categoryId);
                return;
            }

            SelectedValues.Value.Remove(categoryId);
        }

        public override bool GetSelectionState(int categoryId) => SelectedValues.Value.Contains(categoryId);
    }
}