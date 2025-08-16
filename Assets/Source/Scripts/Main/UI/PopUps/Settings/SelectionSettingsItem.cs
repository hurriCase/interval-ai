using System;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SelectionSettingsItem : MonoBehaviour
    {
        [SerializeField] private ButtonTextComponent _buttonTextComponent;
        [SerializeField] private TextMeshProUGUI _settingNameText;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IWindowsController _windowsController;

        private SelectionParameters _parameters;

        internal void Init<TEnum>(ReactiveProperty<TEnum> targetIndexProperty, SelectionParameters parameters)
            where TEnum : unmanaged, Enum
        {
            _parameters = parameters;

            parameters.SettingName = _settingNameText.text;

            targetIndexProperty
                .Subscribe(this, static (enumType, self) =>
                    self.UpdateText<TEnum>(UnsafeEnumConverter<TEnum>.ToInt32(enumType)))
                .RegisterTo(destroyCancellationToken);

            _buttonTextComponent.Button.OnClickAsObservable()
                .Subscribe(this, static (_, self) => self.OpenPopup())
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateText<TEnum>(int index)
            where TEnum : unmanaged, Enum
        {
            _buttonTextComponent.Text.text = _localizationKeysDatabase.GetLocalization<TEnum>(index);
        }

        private void OpenPopup()
        {
            _windowsController.OpenPopUpByType(PopUpType.Selection, _parameters);
        }
    }
}