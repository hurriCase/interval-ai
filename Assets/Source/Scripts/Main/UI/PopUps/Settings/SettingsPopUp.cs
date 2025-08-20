using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.UI.Windows.Base.PopUp;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Settings
{
    internal sealed class SettingsPopUp : PopUpBase
    {
        [SerializeField] private SelectionItem _themeSelectionItem;

        [SerializeField] private CheckboxSettingsItem _isSendNotificationsItem;
        [SerializeField] private CheckboxSettingsItem _isShowTranscriptionItem;
        [SerializeField] private CheckboxSettingsItem _isSwipeEnabledItem;

        [SerializeField] private SelectionItem _languageSelectionItem;
        [SerializeField] private SelectionItem _nativeLanguageSelectionItem;
        [SerializeField] private SelectionItem _learningLanguageSelectionItem;
        [SerializeField] private SelectionItem _showFirstLanguageSelectionItem;
        [SerializeField] private SelectionItem _cardLearnLanguageSelectionItem;
        [SerializeField] private SelectionItem _cardReviewLanguageSelectionItem;
        [SerializeField] private SelectionItem _wordReviewSourceSelectionItem;

        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IAppConfig _appConfig;

        internal override void Init()
        {
            _themeSelectionItem.Init(_settingsRepository.ThemeType.Property);

            _isSendNotificationsItem.Init(_settingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_settingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_settingsRepository.IsSwipeEnabled);

            InitLanguageSelection();

            _wordReviewSourceSelectionItem.Init(_settingsRepository.WordReviewSourceType.Property);
        }

        private void InitLanguageSelection()
        {
            _languageSelectionItem.Init(
                _settingsRepository.SystemLanguage.Property,
                LocalizationController.GetAllLanguages());

            _nativeLanguageSelectionItem.Init(
                _settingsRepository.LanguageByType.Value[LanguageType.Native],
                _appConfig.SupportedLanguages[LanguageType.Native]);

            _learningLanguageSelectionItem.Init(
                _settingsRepository.LanguageByType.Value[LanguageType.Learning],
                _appConfig.SupportedLanguages[LanguageType.Learning]);

            _showFirstLanguageSelectionItem.Init(_settingsRepository.FirstShowPractice.Property);
            _cardLearnLanguageSelectionItem.Init(_settingsRepository.CardLearnPractice.Property);
            _cardReviewLanguageSelectionItem.Init(_settingsRepository.CardReviewPractice.Property);
        }
    }
}