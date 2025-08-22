using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.UI.Components;
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

        [SerializeField] private float _selectionAnimationDuration;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private readonly List<CheckboxTextComponent> _createdSelectionItems = new();

        private DisposableBag _disposableBag;

        public void SetParameters<TValue>(SelectionService<TValue> service)
        {
            _disposableBag.Clear();

            LocalizationController.Language
                .Subscribe((self: this, service), static (_, tuple)
                    => tuple.self._selectionNameText.text = tuple.service.SelectionKey.GetLocalization())
                .AddTo(ref _disposableBag);

            CreateSelections(service);
        }

        internal override async UniTask ShowAsync()
        {
            await base.ShowAsync();

            await AnimatePivotAsync(0f);
        }

        private void CreateSelections<TValue>(SelectionService<TValue> service)
        {
            var selectionValues = service.SelectionValues;

            EnsureElementsCount(selectionValues.Count);

            for (var i = 0; i < selectionValues.Count; i++)
                SetSelectionItem(i, selectionValues[i], service);

            for (var i = 0; i < selectionValues.Count; i++)
                SubscribeToChanges(i, selectionValues[i], service);
        }

        private void EnsureElementsCount(int desiredCount)
        {
            for (var i = desiredCount; i < _createdSelectionItems.Count; i++)
                _createdSelectionItems[i].SetActive(false);

            for (var i = _createdSelectionItems.Count; i < desiredCount; i++)
            {
                var createdItem = Instantiate(_selectionItem, _selectionsContainer);
                _createdSelectionItems.Add(createdItem);
            }

            for (var i = 0; i < desiredCount && i < _createdSelectionItems.Count; i++)
                _createdSelectionItems[i].SetActive(true);
        }

        private void SetSelectionItem<TValue>(int index, TValue selectionValue, SelectionService<TValue> service)
        {
            var selectionItem = _createdSelectionItems[index];

            selectionItem.Text.text = service.GetSelectionName(selectionValue);

            if (service.IsSingleSelection)
                selectionItem.Checkbox.group = _selectionToggleGroup;

            selectionItem.Checkbox.isOn = service.GetSelectionState(selectionValue);
        }

        private void SubscribeToChanges<TValue>(int index, TValue selectionValue, SelectionService<TValue> service)
        {
            var selectionItem = _createdSelectionItems[index];

            selectionItem.Checkbox.OnValueChangedAsObservable()
                .Subscribe((selectionValue, service),
                    static (isOn, tuple) => tuple.service.SetValue(tuple.selectionValue, isOn))
                .AddTo(ref _disposableBag);
        }

        internal override async UniTask HideAsync()
        {
            var tween = AnimatePivotAsync(1f);
            await tween.OnComplete(this, self => self.HideBase());
        }

        private Tween AnimatePivotAsync(float endValue)
            => Tween.UIPivotY(_selectionsContainer, endValue, _selectionAnimationDuration);

        private void HideBase() => base.HideAsync().Forget();

        private void OnDestroy()
        {
            _disposableBag.Dispose();
        }
    }
}