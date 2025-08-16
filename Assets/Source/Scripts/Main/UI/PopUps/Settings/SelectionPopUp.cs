using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    //TODO:<Dmitriy.Sukharev> refactor
    internal sealed class SelectionPopUp : ParameterizedPopUpBase<SelectionParameters>
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

            _selectionNameText.text = Parameters.SettingName;

            CreateSelections();

            base.Show();

            AnimatePivot(0f);
        }

        private void CreateSelections()
        {
            var enumLength = Parameters.SupportValues.Length;

            EnsureElementsCount(enumLength);

            for (var i = 0; i < enumLength; i++)
                SetSelectionItem(_createdSelectionItems[i], Parameters.SupportValues[i]);
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

        private void SetSelectionItem(CheckboxTextComponent selectionItem, int selectionIndex)
        {
            selectionItem.Text.text = Parameters.GetLocalization(_localizationKeysDatabase, selectionIndex);
            selectionItem.Checkbox.OnPointerClickAsObservable()
                .Subscribe((selectionIndex, self: this), static (_, tuple)
                    => tuple.self.ToggleSelection(tuple.selectionIndex))
                .AddTo(ref _disposableBag);

            selectionItem.Checkbox.group = _selectionToggleGroup;
            selectionItem.Checkbox.isOn = selectionIndex == Parameters.SelectedIndex;
        }

        private void ToggleSelection(int selectionIndex)
        {
            Parameters.SetValue(selectionIndex);
            AnimatePivot(0f);
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