using System;
using System.Threading;
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

        internal void Init<TEnum>(
            ReactiveProperty<TEnum> targetProperty,
            ISelectionParameters selectionParameters,
            CancellationToken token)
            where TEnum : unmanaged, Enum
        {
            targetProperty
                .Subscribe(this, static (currentType, self)
                    => self.ChangeSelectionLocalization(currentType))
                .RegisterTo(token);

            _buttonTextComponent.Button.OnClickAsObservable()
                .Subscribe((selectionParameters, self: this), static (_, tuple)
                    => tuple.self._windowsController.OpenPopUpByType(PopUpType.Selection, tuple.selectionParameters))
                .RegisterTo(token);
        }

        private void ChangeSelectionLocalization<TEnum>(TEnum selectionType)
            where TEnum : unmanaged, Enum
        {
            var selectionIndex = UnsafeEnumConverter<TEnum>.ToInt32(selectionType);
            _buttonTextComponent.Text.text = _localizationKeysDatabase.GetLocalization<TEnum>(selectionIndex);
        }
    }
}