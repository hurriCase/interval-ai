using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
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
        [SerializeField] private string _localizationKey;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IWindowsController _windowsController;

        internal void Init<TEnum>(ReactiveProperty<TEnum> targetProperty, TEnum[] customValues = null)
            where TEnum : unmanaged, Enum
        {
            var enumSelectionService = new EnumSelectionService<TEnum>(
                _localizationKeysDatabase,
                targetProperty,
                _localizationKey,
                customValues);

            LocalizationController.Language.SubscribeAndRegister(this, enumSelectionService,
                static (enumSelectionService, self) => self.UpdateLocalization(enumSelectionService));

            enumSelectionService.TargetProperty
                .SubscribeAndRegister(this, static (selectedName, self) => self.UpdateText(selectedName));

            _buttonTextComponent.Button.OnClickAsObservable().SubscribeAndRegister(this, enumSelectionService,
                static (enumSelectionService, self) => self.OpenPopup(enumSelectionService));
        }

        private void UpdateLocalization<TEnum>(EnumSelectionService<TEnum> selectionService)
            where TEnum : unmanaged, Enum
        {
            if (_selectionNameText)
                _selectionNameText.text = selectionService.GetSelectionTitle();

            UpdateText(selectionService.TargetProperty.CurrentValue);
        }

        private void UpdateText<TEnum>(TEnum selectedValue)
            where TEnum : unmanaged, Enum
            => _buttonTextComponent.Text.text = _localizationKeysDatabase.GetLocalizationByValue(selectedValue);

        private void OpenPopup<T>(SelectionService<T> selectionService)
        {
            var selectionPopUp = _windowsController.OpenPopUp<SelectionPopUp>();
            selectionPopUp.SetParameters(selectionService);
        }
    }
}