using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SettingsPopUp : PopUpBase
    {
        [SerializeField] private SelectionSettingsItem _themeButton;
        [SerializeField] private CheckboxSettingsItem _isSendNotificationsItem;
        [SerializeField] private CheckboxSettingsItem _isShowTranscriptionItem;
        [SerializeField] private CheckboxSettingsItem _isSwipeEnabledItem;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal override void Init()
        {
            _themeButton.Init(_settingsRepository.CurrentTheme.Property, destroyCancellationToken);
            _isSendNotificationsItem.Init(_settingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_settingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_settingsRepository.IsSwipeEnabled);
        }
    }
}