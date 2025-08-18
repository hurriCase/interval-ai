using R3;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal readonly struct SingleSelectionParameters : ISelectionParameters
    {
        public string SelectionName { get; }
        public SelectionData[] SelectionValues { get; }
        public bool IsSingleSelection { get; }

        public ReactiveProperty<int> SelectedValue { get; }

        public SingleSelectionParameters(
            string selectionName,
            SelectionData[] selectionValues,
            int selectedValue)
        {
            SelectionName = selectionName;
            SelectionValues = selectionValues;
            IsSingleSelection = true;
            SelectedValue = new ReactiveProperty<int>(selectedValue);
        }

        public void SetValue(int value, bool isSelected)
        {
            if (isSelected)
                SelectedValue.Value = value;
        }

        public bool GetSelectionState(int value) => SelectedValue.Value == value;
    }
}