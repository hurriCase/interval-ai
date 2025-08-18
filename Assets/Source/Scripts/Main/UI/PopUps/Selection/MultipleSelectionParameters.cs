using System.Collections.Generic;
using R3;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal readonly struct MultipleSelectionParameters : ISelectionParameters
    {
        public string SelectionName { get; }
        public SelectionData[] SelectionValues { get; }
        public bool IsSingleSelection { get; }
        public ReactiveProperty<List<int>> SelectedValues { get; }

        public MultipleSelectionParameters(
            string selectionName,
            SelectionData[] selectionValues,
            List<int> selectedValues)
        {
            SelectionName = selectionName;
            SelectionValues = selectionValues;
            IsSingleSelection = false;

            SelectedValues = new ReactiveProperty<List<int>>(selectedValues);
        }

        public void SetValue(int value, bool isSelected)
        {
            if (isSelected)
            {
                if (SelectedValues.Value.Contains(value) is false)
                    SelectedValues.Value.Add(value);
                return;
            }

            SelectedValues.Value.Remove(value);
        }

        public bool GetSelectionState(int value) => SelectedValues.Value.Contains(value);
    }
}