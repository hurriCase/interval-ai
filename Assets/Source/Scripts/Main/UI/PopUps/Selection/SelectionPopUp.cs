using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using Source.Scripts.Core.Others;
using Source.Scripts.UI.Components.Checkbox;
using Source.Scripts.UI.Data;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class SelectionPopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _selectionNameText;

        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private CheckboxTextComponent _selectionItem;
        [SerializeField] private ToggleGroup _selectionToggleGroup;

        private IAnimationsConfig _animationsConfig;

        private UIPool<CheckboxTextComponent> _selectionPool;

        private DisposableBag _disposableBag;

        [Inject]
        internal void Inject(IAnimationsConfig animationsConfig)
        {
            _animationsConfig = animationsConfig;
        }

        internal override void Init()
        {
            _selectionPool = new UIPool<CheckboxTextComponent>(_selectionItem, _selectionsContainer);
        }

        public void SetParameters<TValue>(ISelectionService<TValue> service)
        {
            _disposableBag.Clear();

            _selectionNameText.text = service.GetSelectionTitle();

            CreateSelections(service);
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            await AnimatePivotAsync(0f);
        }

        private void CreateSelections<TValue>(ISelectionService<TValue> service)
        {
            var selectionValues = service.SelectionValues;

            _selectionPool.EnsureCount(selectionValues.Count);

            for (var i = 0; i < selectionValues.Count; i++)
                SetSelectionItem(i, selectionValues[i], service);

            for (var i = 0; i < selectionValues.Count; i++)
                SubscribeToChanges(i, selectionValues[i], service);
        }

        private void SetSelectionItem<TValue>(int index, TValue selectionValue, ISelectionService<TValue> service)
        {
            var selectionItem = _selectionPool.PooledItems[index];

            selectionItem.Text.text = service.GetSelectionName(selectionValue);

            if (service.IsSingleSelection)
                selectionItem.Checkbox.group = _selectionToggleGroup;

            selectionItem.Checkbox.isOn = service.GetSelectionState(selectionValue);
        }

        private void SubscribeToChanges<TValue>(int index, TValue selectionValue, ISelectionService<TValue> service)
        {
            var selectionItem = _selectionPool.PooledItems[index];

            selectionItem.Checkbox.OnValueChangedAsObservable()
                .Subscribe((selectionValue, service),
                    static (isOn, tuple) => tuple.service.SetValue(tuple.selectionValue, isOn))
                .AddTo(ref _disposableBag);
        }

        internal override async UniTask HideAsync()
        {
            await AnimatePivotAsync(1f);

            base.HideAsync().Forget();
        }

        private Tween AnimatePivotAsync(float endValue)
            => Tween.UIPivotY(_selectionsContainer, endValue, _animationsConfig.SelectionSwitchDuration);

        private void OnDestroy()
        {
            _disposableBag.Dispose();
        }
    }
}