using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Components.Button;
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
        [SerializeField] private ButtonComponent _changeSettingsButton;

        private IGenerationSettingsRepository _generationSettingsRepository;
        private ILanguageSettingsRepository _languageSettingsRepository;
        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(
            IGenerationSettingsRepository generationSettingsRepository,
            ILanguageSettingsRepository languageSettingsRepository,
            ILocalizationKeysDatabase localizationKeysDatabase,
            IWindowsController windowsController)
        {
            _generationSettingsRepository = generationSettingsRepository;
            _languageSettingsRepository = languageSettingsRepository;
            _localizationKeysDatabase = localizationKeysDatabase;
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _generationSettingsRepository.NewWordsPercentage
                .SubscribeAndRegister(this, static (percent, self) => self.SetPercentText(percent));

            _generationSettingsRepository.TranslateFromLanguageType
                .SubscribeAndRegister(this, static (percent, self) => self.SetLanguageTypeText(percent));

            _generationSettingsRepository.IsHighlightNewWords
                .SubscribeAndRegister(this, static (percent, self) => self.SetIsHighlightText(percent));

            _changeSettingsButton.OnClickAsObservable().SubscribeUntilDestroy(this,
                static self => self._windowsController.OpenPopUpByType(PopUpType.GenerationSettings));
        }

        private void SetPercentText(float percent)
        {
            var newWordsLocalization = _localizationKeysDatabase.GetLocalization(LocalizationType.NewWordPercent);
            _wordPercentText.SetTextFormat(newWordsLocalization, Mathf.RoundToInt(percent * 100));
        }

        private void SetLanguageTypeText(LanguageType languageType)
        {
            var systemLanguage = _languageSettingsRepository.LanguageByType.CurrentValue[languageType];
            var languageLocalization = _localizationKeysDatabase.GetLanguageLocalization(systemLanguage);
            var translateFromLocalization =
                _localizationKeysDatabase.GetLocalization(LocalizationType.ShowOnLanguage);

            _showOnLanguageText.SetTextFormat(translateFromLocalization, languageLocalization);
        }

        private void SetIsHighlightText(bool isHighlight)
        {
            var highlightType = isHighlight ? LocalizationType.Highlight : LocalizationType.NotHighlight;
            var isHighlightLocalization = _localizationKeysDatabase.GetLocalization(highlightType);
            var isHighlightNewWordsLocalization =
                _localizationKeysDatabase.GetLocalization(LocalizationType.IsHighlightNewWords);

            _isHighlightText.SetTextFormat(isHighlightNewWordsLocalization, isHighlightLocalization);
        }
    }
}