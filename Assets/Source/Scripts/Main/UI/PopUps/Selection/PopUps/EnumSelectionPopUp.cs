using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.Selection.PopUps
{
    internal sealed class EnumSelectionPopUp : SelectionPopUp
    {
        [SerializeField] private ToggleGroup _selectionToggleGroup;

        protected override void OnSetupSelectionItem(StateToggle selectionItem)
        {
            selectionItem.group = _selectionToggleGroup;
        }

        protected override void SubscribeSelection<TValue>(
            Toggle selectionItem,
            TValue selectionValue,
            ISelectionService<TValue> service)
        {
            selectionItem.OnPointerClickAsObservable()
                .Do((selectionValue, service), static (_, tuple) => tuple.service.SetValue(tuple.selectionValue, true))
                .Subscribe(this, static (_, self) => self.HideAsync().Forget())
                .AddTo(ref disposableBag);
        }
    }
}