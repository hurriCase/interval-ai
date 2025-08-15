using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomBehaviours;
using CustomUtils.Runtime.Extensions;
using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SelectionBehaviour : RectTransformBehaviour
    {
        [SerializeField] private TextMeshProUGUI _selectionNameText;

        [SerializeField] private RectTransform _selectionsContainer;
        [SerializeField] private CheckboxTextComponent _selectionItem;

        [SerializeField] private float _animationDuration;

        [Inject] private ISelectionLocalizationKeysDatabase _selectionLocalizationKeysDatabase;

        private readonly List<CheckboxTextComponent> _createdSelectionItems = new();

        private DisposableBag _disposableBag;

        internal void Show<TEnum>(ReactiveProperty<TEnum> property, string settingName)
            where TEnum : unmanaged, Enum
        {
            _disposableBag.Clear();

            _selectionNameText.text = settingName;

            CreateSelections(property);

            ToggleVisibility(true);
        }

        private void CreateSelections<TEnum>(ReactiveProperty<TEnum> property)
            where TEnum : unmanaged, Enum
        {
            var enumValues = Enum.GetValues(typeof(TEnum));
            EnsureElementsCount(enumValues.Length);
            for (var i = 0; i < enumValues.Length; i++)
            {
                var enumType = (TEnum)enumValues.GetValue(i);

                SetSelectionItem(property, _createdSelectionItems[i], enumType);
            }
        }

        private void ToggleVisibility(bool isShow)
            => Tween.UIPivotY(RectTransform, isShow ? 0f : 1f, _animationDuration);

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

        private void SetSelectionItem<TEnum>(
            ReactiveProperty<TEnum> property,
            CheckboxTextComponent selectionItem,
            TEnum selectionType)
            where TEnum : unmanaged, Enum
        {
            selectionItem.Text.text = _selectionLocalizationKeysDatabase.GetLocalization(selectionType);
            selectionItem.Checkbox.OnPointerClickAsObservable()
                .Subscribe((property, selectionType, self: this), static (_, tuple) =>
                {
                    tuple.property.Value = tuple.selectionType;
                    tuple.self.ToggleVisibility(false);
                })
                .AddTo(ref _disposableBag);
        }

        private void OnDestroy()
        {
            _disposableBag.Dispose();
        }
    }
}