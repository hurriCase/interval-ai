using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Text;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Selection.LocalizationData;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Generation.Behaviours
{
    internal sealed class CurrentSettingsBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wordPercentText;
        [SerializeField] private TextMeshProUGUI _showOnLanguageText;
        [SerializeField] private TextMeshProUGUI _isHighlightText;
        [SerializeField] private LocalizationKey _wordPercentKey;
        [SerializeField] private LocalizationKey _showOnLanguageKey;
        [SerializeField] private LocalizationKey _isHighlightNewWordsKey;
        [SerializeField] private LocalizationKey _highlightKey;
        [SerializeField] private LocalizationKey _notHighlightKey;

        [SerializeField] private ThemeButton _changeSettingsButton;

        [SerializeField] private LanguageLocalizationConfig _languageLocalizationConfig;

        private IGenerationSettingsRepository _generationSettingsRepository;
        private ILanguageSettingsRepository _languageSettingsRepository;
        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(
            IGenerationSettingsRepository generationSettingsRepository,
            ILanguageSettingsRepository languageSettingsRepository,
            IWindowsController windowsController)
        {
            _generationSettingsRepository = generationSettingsRepository;
            _languageSettingsRepository = languageSettingsRepository;
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _generationSettingsRepository.NewWordsPercentage
                .SubscribeUntilDestroy(this, static (percent, self) => self.SetPercentText(percent));

            _generationSettingsRepository.TranslateFromLanguageType
                .SubscribeUntilDestroy(this, static (percent, self) => self.SetLanguageTypeText(percent));

            _generationSettingsRepository.IsHighlightNewWords
                .SubscribeUntilDestroy(this, static (percent, self) => self.SetIsHighlightText(percent));

            _windowsController.BindPopUpOpen(_changeSettingsButton, PopUpType.GenerationSettings);
        }

        private void SetPercentText(float percent)
        {
            var newWordsLocalization = _wordPercentKey.GetLocalization();
            _wordPercentText.SetTextFormat(newWordsLocalization, Mathf.RoundToInt(percent * 100));
        }

        private void SetLanguageTypeText(LanguageType languageType)
        {
            var systemLanguage = _languageSettingsRepository.LanguageByType.CurrentValue[languageType];
            var languageLocalization = _languageLocalizationConfig.GetLocalization(systemLanguage);
            var translateFromLocalization = _showOnLanguageKey.GetLocalization();

            _showOnLanguageText.SetTextFormat(translateFromLocalization, languageLocalization);
        }

        private void SetIsHighlightText(bool isHighlight)
        {
            var highlightType = isHighlight ? _highlightKey : _notHighlightKey;
            var isHighlightLocalization = highlightType.GetLocalization();
            var isHighlightNewWordsLocalization = _isHighlightNewWordsKey.GetLocalization();

            _isHighlightText.SetTextFormat(isHighlightNewWordsLocalization, isHighlightLocalization);
        }
    }
}