using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection.LocalizationData;
using Source.Scripts.Main.UI.PopUps.Selection.PopUps;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Selection
{
    internal sealed class SelectionButton : MonoBehaviour
    {
        [SerializeField] private ThemeButton _buttonComponent;
        [SerializeField] private TextMeshProUGUI _selectionNameText;
        [SerializeField] private TextMeshProUGUI _currentSelectionText;
        [SerializeField] private LocalizationKey _titleLocalizationKey;
        [SerializeField] private EnumLocalizationDataBase _localizationData;

        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IWindowsController windowsController, IObjectResolver objectResolver)
        {
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal void Init<TEnum>(ReactiveProperty<TEnum> targetProperty, IReadOnlyList<TEnum> customValues = null)
            where TEnum : unmanaged, Enum
        {
            _objectResolver.Inject(_localizationData);

            var enumSelectionService = new EnumSelectionService<TEnum>(
                targetProperty, _localizationData, customValues);

            LocalizationController.Language.SubscribeUntilDestroy(this, enumSelectionService,
                static (enumSelectionService, self) => self.UpdateLocalization(enumSelectionService));

            enumSelectionService.TargetProperty
                .SubscribeUntilDestroy(this, static (selectedName, self) => self.UpdateText(selectedName));

            _buttonComponent.OnClickAsObservable().SubscribeUntilDestroy(this, enumSelectionService,
                static (enumSelectionService, self) => self.OpenPopup(enumSelectionService));
        }

        private void UpdateLocalization<TEnum>(EnumSelectionService<TEnum> selectionService)
            where TEnum : unmanaged, Enum
        {
            if (_selectionNameText)
                _selectionNameText.text = _titleLocalizationKey.GetLocalization();

            UpdateText(selectionService.TargetProperty.CurrentValue);
        }

        private void UpdateText<TEnum>(TEnum selectedValue)
            where TEnum : unmanaged, Enum
        {
            _currentSelectionText.text = _localizationData.GetLocalization(selectedValue);
        }

        private void OpenPopup<T>(ISelectionService<T> selectionService)
        {
            var selectionPopUp = _windowsController.OpenPopUp<EnumSelectionPopUp>();
            selectionPopUp.SetParameters(selectionService, _titleLocalizationKey.GetLocalization());
        }
    }
}