using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class SelectionItem : MonoBehaviour
    {
        [SerializeField] private ButtonTextComponent _buttonTextComponent;
        [SerializeField] private TextMeshProUGUI _selectionNameText;
        [SerializeField] private string _selectionNameKey;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IWindowsController _windowsController;

        private SingleSelectionParameters _parameters;

        internal void Init<TEnum>(
            ReactiveProperty<TEnum> targetProperty,
            TEnum[] customValues = null,
            EnumMode enumMode = EnumMode.SkipFirst)
            where TEnum : unmanaged, Enum
        {
            var selectionValues = customValues is null
                ? CreateSelectionValues<TEnum>(enumMode)
                : CreateSelectionValues(customValues);

            var selectionName = _selectionNameKey.GetLocalization();
            if (_selectionNameText)
                _selectionNameText.text = selectionName;

            _parameters = new SingleSelectionParameters(
                selectionName,
                selectionValues,
                UnsafeEnumConverter<TEnum>.ToInt32(targetProperty.Value));

            SubscribeToEvents(targetProperty);
        }

        private SelectionData[] CreateSelectionValues<TEnum>(EnumMode enumMode)
            where TEnum : unmanaged, Enum
        {
            var enumValues = Enum.GetValues(typeof(TEnum));

            var startIndex = enumMode == EnumMode.SkipFirst ? 1 : 0;
            var selectionValues = new SelectionData[enumValues.Length - startIndex];

            for (var i = startIndex; i < enumValues.Length; i++)
            {
                var selectionName = _localizationKeysDatabase.GetLocalization(typeof(TEnum), i);
                selectionValues[i - startIndex] = new SelectionData(selectionName, i);
            }

            return selectionValues;
        }

        private SelectionData[] CreateSelectionValues<TEnum>(IReadOnlyList<TEnum> customValues)
            where TEnum : unmanaged, Enum
        {
            var selectionValues = new SelectionData[customValues.Count];
            for (var i = 0; i < customValues.Count; i++)
            {
                var enumIndex = UnsafeEnumConverter<TEnum>.ToInt32(customValues[i]);
                var selectionName = _localizationKeysDatabase.GetLocalization(typeof(TEnum), enumIndex);
                selectionValues[i] = new SelectionData(selectionName, enumIndex);
            }

            return selectionValues;
        }

        private void SubscribeToEvents<TEnum>(ReactiveProperty<TEnum> targetProperty)
            where TEnum : unmanaged, Enum
        {
            targetProperty
                .Subscribe(this, static (enumType, self) => self.UpdateText(enumType))
                .RegisterTo(destroyCancellationToken);

            _parameters.SelectedValue
                .Subscribe(targetProperty, static (newValue, targetProperty)
                    => targetProperty.Value = UnsafeEnumConverter<TEnum>.FromInt32(newValue))
                .RegisterTo(destroyCancellationToken);

            _buttonTextComponent.Button.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.OpenPopup())
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateText<TEnum>(TEnum enumType) where TEnum : unmanaged, Enum
        {
            var enumIndex = UnsafeEnumConverter<TEnum>.ToInt32(enumType);
            _buttonTextComponent.Text.text = _localizationKeysDatabase.GetLocalization(typeof(TEnum), enumIndex);
        }

        private void OpenPopup()
        {
            _windowsController.OpenPopUpByType<ISelectionParameters>(PopUpType.Selection, _parameters);
        }
    }
}