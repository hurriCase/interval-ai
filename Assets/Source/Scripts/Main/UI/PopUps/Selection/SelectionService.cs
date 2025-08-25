using System.Collections.Generic;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal abstract class SelectionService<TValue>
    {
        public IReadOnlyList<TValue> SelectionValues { get; protected set; }
        public bool IsSingleSelection { get; }

        internal SelectionService(
            IReadOnlyList<TValue> selectionValues,
            bool isSingleSelection)
        {
            SelectionValues = selectionValues;
            IsSingleSelection = isSingleSelection;
        }

        public abstract string GetSelectionName(TValue value);
        public abstract string GetSelectionTitle();
        public abstract void SetValue(TValue value, bool isSelected);
        public abstract bool GetSelectionState(TValue value);
    }
}