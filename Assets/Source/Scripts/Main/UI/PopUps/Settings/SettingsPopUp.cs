using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.UI.Theme.Base;
using CustomUtils.Unsafe.CustomUtils.Unsafe;
using R3;
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
            var themeParams = CreateSelectionParameters<ThemeType>(
                _settingsRepository.CurrentTheme.Property.AsIndexProperty(destroyCancellationToken));

            _themeSelectionItem.Init(_settingsRepository.CurrentTheme.Property, themeParams);

            var languageParams = CreateSelectionParameters(
                _settingsRepository.SystemLanguage.Property.AsIndexProperty(destroyCancellationToken),
                _appConfig.SupportedLanguages[LanguageType.Native],
                enumMode: EnumMode.Default);

            _languageSelectionItem.Init(_settingsRepository.SystemLanguage.Property, languageParams);

            _isSendNotificationsItem.Init(_settingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_settingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_settingsRepository.IsSwipeEnabled);
        }

        private SelectionParameters CreateSelectionParameters<TEnum>(
            ReactiveProperty<int> property,
            TEnum[] customValues = null,
            EnumMode enumMode = EnumMode.SkipFirst)
            where TEnum : unmanaged, Enum
        {
            var values = customValues ?? (TEnum[])Enum.GetValues(typeof(TEnum));
            var startIndex = enumMode == EnumMode.SkipFirst ? 1 : 0;

            var supportValues = new int[values.Length - startIndex];
            for (var i = startIndex; i < values.Length; i++)
                supportValues[i - startIndex] = UnsafeEnumConverter<TEnum>.ToInt32(values[i]);

            return new SelectionParameters(
                property.Value,
                supportValues,
                property,
                (database, index) => database.GetLocalization<TEnum>(index));
        }
    }
}