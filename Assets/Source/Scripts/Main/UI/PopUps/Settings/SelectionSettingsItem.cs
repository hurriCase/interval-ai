using System;
using System.Threading;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
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
        [SerializeField] private SelectionBehaviour _selectionBehaviour;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ISelectionLocalizationKeysDatabase _selectionLocalizationKeysDatabase;

        internal void Init<TEnum>(ReactiveProperty<TEnum> targetProperty, CancellationToken token)
            where TEnum : unmanaged, Enum
        {
            targetProperty
                .Subscribe(this, static (currentTheme, self)
                    => self.ChangeSelectionLocalization(currentTheme))
                .RegisterTo(token);

            _buttonTextComponent.Button.OnClickAsObservable()
                .Subscribe((targetProperty, self: this), static (_, tuple)
                    => tuple.self._selectionBehaviour.Show(tuple.targetProperty, tuple.self._settingNameText.text))
                .RegisterTo(token);
        }

        private void ChangeSelectionLocalization<TEnum>(TEnum selectionType)
        {
            _buttonTextComponent.Text.text = _selectionLocalizationKeysDatabase.GetLocalization(selectionType);
        }
    }
}