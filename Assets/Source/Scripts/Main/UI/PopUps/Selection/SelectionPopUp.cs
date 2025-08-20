using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using PrimeTween;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
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
            LocalizationController.Language
                .Subscribe((self: this, service), static (_, tuple)
                    => tuple.self._selectionNameText.text = tuple.service.SelectionKey.GetLocalization())
                .RegisterTo(destroyCancellationToken);

            CreateSelections(service);
        }

        internal override void Show()
        {
            _disposableBag.Clear();

            base.Show();

            AnimatePivot(0f);
        }

        private void CreateSelections<TValue>(SelectionService<TValue> service)
        {
            var selectionValues = service.SelectionValues;

            EnsureElementsCount(selectionValues.Count);

            for (var i = 0; i < selectionValues.Count; i++)
                SetSelectionItem(i, selectionValues[i], service);
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

            selectionItem.Checkbox.OnValueChangedAsObservable()
                .Subscribe((selectionData: selectionValue, service),
                    static (isOn, tuple) => tuple.service.SetValue(tuple.selectionData, isOn))
                .AddTo(ref _disposableBag);
        }

        internal override void Hide()
        {
            var tween = AnimatePivot(1f);
            tween.OnComplete(this, self => self.HideBase());
        }

        private Tween AnimatePivot(float endValue)
            => Tween.UIPivotY(_selectionsContainer, endValue, _selectionAnimationDuration);

        private void HideBase() => base.Hide();

        private void OnDestroy()
        {
            _disposableBag.Dispose();
        }
    }
}