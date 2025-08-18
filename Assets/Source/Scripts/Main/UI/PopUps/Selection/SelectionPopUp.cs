using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
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
    internal sealed class SelectionPopUp : ParameterizedPopUpBase<ISelectionParameters>
    {
        [SerializeField] private TextMeshProUGUI _selectionNameText;

        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private CheckboxTextComponent _selectionItem;
        [SerializeField] private ToggleGroup _selectionToggleGroup;

        [SerializeField] private float _selectionAnimationDuration;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        private readonly List<CheckboxTextComponent> _createdSelectionItems = new();

        private DisposableBag _disposableBag;

        internal override void Show()
        {
            _disposableBag.Clear();

            _selectionNameText.text = Parameters.SelectionName;

            CreateSelections();

            base.Show();

            AnimatePivot(0f);
        }

        private void CreateSelections()
        {
            var selectionValues = Parameters.SelectionValues;

            EnsureElementsCount(selectionValues.Length);

            for (var i = 0; i < selectionValues.Length; i++)
                SetSelectionItem(i, selectionValues[i]);
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

        private void SetSelectionItem(int index, SelectionData selectionData)
        {
            var selectionItem = _createdSelectionItems[index];

            selectionItem.Text.text = selectionData.SelectionName;

            if (Parameters.IsSingleSelection)
                selectionItem.Checkbox.group = _selectionToggleGroup;

            selectionItem.Checkbox.isOn = Parameters.GetSelectionState(selectionData.Value);

            selectionItem.Checkbox.OnValueChangedAsObservable()
                .Subscribe((selectionData.Value, self: this), static (isOn, tuple)
                    => tuple.self.Parameters.SetValue(tuple.Value, isOn))
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