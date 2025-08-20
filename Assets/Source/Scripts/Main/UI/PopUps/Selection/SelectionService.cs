using System.Collections.Generic;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal abstract class SelectionService<TValue>
    {
        public IReadOnlyList<TValue> SelectionValues { get; protected set; }
        public string SelectionKey { get; private set; }
        public bool IsSingleSelection { get; }

        internal SelectionService(
            IReadOnlyList<TValue> selectionValues,
            bool isSingleSelection,
            string selectionKey = null)
        {
            SelectionValues = selectionValues;
            IsSingleSelection = isSingleSelection;
            SelectionKey = selectionKey;
        }

        public abstract string GetSelectionName(TValue value);
        public abstract void SetValue(TValue value, bool isSelected);
        public abstract bool GetSelectionState(TValue value);
    }
}