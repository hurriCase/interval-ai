using R3;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.Selection.PopUps
{
    internal sealed class CategorySelectionPopUp : SelectionPopUp
    {
        protected override void SubscribeSelection<TValue>(
            Toggle selectionItem,
            TValue selectionValue,
            ISelectionService<TValue> service)
        {
            selectionItem.OnValueChangedAsObservable()
                .Subscribe((selectionValue, service), static (isOn, tuple) =>
                    tuple.service.SetValue(tuple.selectionValue, isOn))
                .AddTo(ref disposableBag);
        }
    }
}