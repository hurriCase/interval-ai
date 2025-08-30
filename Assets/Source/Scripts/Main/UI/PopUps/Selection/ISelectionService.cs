using System.Collections.Generic;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal interface ISelectionService<TValue>
    {
        public IReadOnlyList<TValue> SelectionValues { get; }
        public bool IsSingleSelection { get; }
        public string GetSelectionName(TValue value);
        public string GetSelectionTitle();
        public void SetValue(TValue value, bool isSelected);
        public bool GetSelectionState(TValue value);
    }
}