using CustomUtils.Runtime.Animations;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Main.UI.PopUps.Selection.PopUps
{
    internal abstract class SelectionPopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _selectionNameText;

        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private StateToggle _selectionItem;

        [SerializeField] private PivotAnimation<VisibilityState> _pivotAnimation;

        private UIPool<StateToggle> _selectionPool;

        protected DisposableBag disposableBag;

        internal override void Init()
        {
            _selectionPool = new UIPool<StateToggle>(_selectionItem, _selectionsContainer);
        }

        public void SetParameters<TValue>(ISelectionService<TValue> service, string title)
        {
            disposableBag.Clear();

            _selectionNameText.text = title;

            CreateSelections(service);
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            await _pivotAnimation.PlayAnimation(VisibilityState.Visible);
        }

        internal override async UniTask HideAsync()
        {
            await _pivotAnimation.PlayAnimation(VisibilityState.Hidden);

            base.HideAsync().Forget();
        }

        private void CreateSelections<TValue>(ISelectionService<TValue> service)
        {
            var selectionValues = service.SelectionValues;

            _selectionPool.EnsureCount(selectionValues.Count);

            for (var i = 0; i < selectionValues.Count; i++)
                SetupSelectionItem(i, selectionValues[i], service);

            for (var i = 0; i < selectionValues.Count; i++)
                SubscribeToSelection(i, selectionValues[i], service);
        }

        private void SetupSelectionItem<TValue>(int index, TValue selectionValue, ISelectionService<TValue> service)
        {
            var selectionItem = _selectionPool.ActiveItems[index];

            selectionItem.Text.text = service.GetSelectionName(selectionValue);
            selectionItem.isOn = service.GetSelectionState(selectionValue);

            OnSetupSelectionItem(selectionItem);
        }

        protected virtual void OnSetupSelectionItem(StateToggle selectionItem) { }

        private void SubscribeToSelection<TValue>(int index, TValue selectionValue, ISelectionService<TValue> service)
        {
            var selectionItem = _selectionPool.ActiveItems[index];

            SubscribeSelection(selectionItem, selectionValue, service);
        }

        protected abstract void SubscribeSelection<TValue>(
            Toggle selectionItem,
            TValue selectionValue,
            ISelectionService<TValue> service);

        private void OnDestroy()
        {
            disposableBag.Dispose();
        }
    }
}