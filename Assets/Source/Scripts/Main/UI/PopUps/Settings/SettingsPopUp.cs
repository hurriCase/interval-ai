using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.UI.Theme.Base;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SettingsPopUp : PopUpBase
    {
        [SerializeField] private SelectionSettingsItem _themeSelectionItem;
        [SerializeField] private SelectionSettingsItem _languageSelectionItem;
        [SerializeField] private CheckboxSettingsItem _isSendNotificationsItem;
        [SerializeField] private CheckboxSettingsItem _isShowTranscriptionItem;
        [SerializeField] private CheckboxSettingsItem _isSwipeEnabledItem;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IAppConfig _appConfig;

        internal override void Init()
        {
            var themeParameters = new SelectionParameters<ThemeType>(
                _settingsRepository.CurrentTheme.Property,
                "");

            _themeSelectionItem.Init(
                _settingsRepository.CurrentTheme.Property,
                themeParameters,
                destroyCancellationToken);

            var languageParameters =
                new SelectionParameters<SystemLanguage>(
                    _settingsRepository.SystemLanguage.Property,
                    "",
                    _appConfig.SupportedLanguages[LanguageType.Native],
                    EnumMode.Default);

            _languageSelectionItem.Init(
                _settingsRepository.SystemLanguage.Property,
                languageParameters,
                destroyCancellationToken);

            _isSendNotificationsItem.Init(_settingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_settingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_settingsRepository.IsSwipeEnabled);
        }
    }
}