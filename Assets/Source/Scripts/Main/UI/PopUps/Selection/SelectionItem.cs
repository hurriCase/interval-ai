using System;
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

        private SelectionParameters _parameters;

        internal void Init<TEnum>(
            ReactiveProperty<TEnum> targetIndexProperty,
            TEnum[] customValues = null,
            EnumMode enumMode = EnumMode.SkipFirst)
            where TEnum : unmanaged, Enum
        {
            _parameters = new SelectionParameters();

            if (_selectionNameText)
                _selectionNameText.text = _selectionNameKey.GetLocalization();

            _parameters.SetParameters(
                _selectionNameKey.GetLocalization(),
                targetIndexProperty,
                customValues,
                enumMode,
                destroyCancellationToken);

            targetIndexProperty
                .Subscribe(this, static (enumType, self) => self.UpdateText(enumType))
                .RegisterTo(destroyCancellationToken);

            _parameters.SelectionIndex
                .Subscribe(targetIndexProperty, static (newValue, targetProperty)
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
            _windowsController.OpenPopUpByType(PopUpType.Selection, _parameters);
        }
    }
}