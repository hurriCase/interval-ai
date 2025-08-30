using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.PopUps.Selection;
using Source.Scripts.UI.Windows.Base;
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

        private ILanguageSettingsRepository _languageSettingsRepository;
        private IPracticeSettingsRepository _practiceSettingsRepository;
        private IUISettingsRepository _iuiSettingsRepository;
        private IAppConfig _appConfig;

        [Inject]
        internal void Inject(
            ILanguageSettingsRepository languageSettingsRepository,
            IPracticeSettingsRepository practiceSettingsRepository,
            IUISettingsRepository iuiSettingsRepository,
            IAppConfig appConfig)
        {
            _languageSettingsRepository = languageSettingsRepository;
            _practiceSettingsRepository = practiceSettingsRepository;
            _iuiSettingsRepository = iuiSettingsRepository;
            _appConfig = appConfig;
        }

        internal override void Init()
        {
            _themeSelectionItem.Init(_iuiSettingsRepository.ThemeType.Property);

            _isSendNotificationsItem.Init(_iuiSettingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_iuiSettingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_iuiSettingsRepository.IsSwipeEnabled);

            InitLanguageSelection();

            _wordReviewSourceSelectionItem.Init(_practiceSettingsRepository.WordReviewSourceType.Property);
        }

        private void InitLanguageSelection()
        {
            _languageSelectionItem.Init(
                _languageSettingsRepository.SystemLanguage.Property,
                LocalizationController.GetAllLanguages());

            _nativeLanguageSelectionItem.Init(
                _languageSettingsRepository.LanguageProperties[LanguageType.Native],
                _appConfig.SupportedLanguages[LanguageType.Native]);

            _learningLanguageSelectionItem.Init(
                _languageSettingsRepository.LanguageProperties[LanguageType.Learning],
                _appConfig.SupportedLanguages[LanguageType.Learning]);

            _showFirstLanguageSelectionItem.Init(_languageSettingsRepository.FirstShowLanguageType.Property);
            _cardLearnLanguageSelectionItem.Init(_languageSettingsRepository.CardLearnLanguageType.Property);
            _cardReviewLanguageSelectionItem.Init(_languageSettingsRepository.CardReviewLanguageType.Property);
        }
    }
}