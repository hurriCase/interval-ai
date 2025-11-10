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
        [SerializeField] private SelectionButton _themeSelection;

        [SerializeField] private CheckboxSettingsItem _isSendNotificationsItem;
        [SerializeField] private CheckboxSettingsItem _isShowTranscriptionItem;
        [SerializeField] private CheckboxSettingsItem _isSwipeEnabledItem;

        [SerializeField] private SelectionButton _languageSelection;
        [SerializeField] private SelectionButton _nativeLanguageSelection;
        [SerializeField] private SelectionButton _learningLanguageSelection;
        [SerializeField] private SelectionButton _showFirstLanguageSelection;
        [SerializeField] private SelectionButton _cardLearnLanguageSelection;
        [SerializeField] private SelectionButton _cardReviewLanguageSelection;
        [SerializeField] private SelectionButton _wordReviewSourceSelection;

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
            _themeSelection.Init(_iuiSettingsRepository.ThemeType.Property);

            _isSendNotificationsItem.Init(_iuiSettingsRepository.IsSendNotifications);
            _isShowTranscriptionItem.Init(_iuiSettingsRepository.IsShowTranscription);
            _isSwipeEnabledItem.Init(_iuiSettingsRepository.IsSwipeEnabled);

            InitLanguageSelection();

            _wordReviewSourceSelection.Init(_practiceSettingsRepository.WordReviewSourceType.Property);
        }

        private void InitLanguageSelection()
        {
            _languageSelection.Init(
                _languageSettingsRepository.SystemLanguage.Property,
                LocalizationRegistry.Instance.SupportedLanguages);

            _nativeLanguageSelection.Init(
                _languageSettingsRepository.LanguageProperties[LanguageType.Native],
                _appConfig.SupportedLanguages[LanguageType.Native]);

            _learningLanguageSelection.Init(
                _languageSettingsRepository.LanguageProperties[LanguageType.Learning],
                _appConfig.SupportedLanguages[LanguageType.Learning]);

            _showFirstLanguageSelection.Init(_languageSettingsRepository.FirstShowLanguageType.Property);
            _cardLearnLanguageSelection.Init(_languageSettingsRepository.CardLearnLanguageType.Property);
            _cardReviewLanguageSelection.Init(_languageSettingsRepository.CardReviewLanguageType.Property);
        }
    }
}