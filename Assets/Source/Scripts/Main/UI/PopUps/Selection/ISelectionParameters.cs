namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal interface ISelectionParameters
    {
        string SelectionName { get; }
        SelectionData[] SelectionValues { get; }
        bool IsSingleSelection { get; }

        void SetValue(int value, bool isSelected);
        bool GetSelectionState(int value);
    }
}