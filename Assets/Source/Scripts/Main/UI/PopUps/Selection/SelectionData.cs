namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal readonly struct SelectionData
    {
        internal string SelectionName { get; }
        internal int Value { get; }

        internal SelectionData(string selectionName, int value)
        {
            SelectionName = selectionName;
            Value = value;
        }
    }
}