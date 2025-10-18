using System;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private ThemeButton _startPracticeButton;

        [SerializeField] private TextMeshProUGUI _learnGoalText;
        [SerializeField] private TextMeshProUGUI _repetitionText;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IProgressRepository _progressRepository;
        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(
            ILocalizationKeysDatabase localizationKeysDatabase,
            IProgressRepository progressRepository,
            IWindowsController windowsController)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
            _progressRepository = progressRepository;
            _windowsController = windowsController;
        }

        internal void Init()
        {
            _plusMinusBehaviour.Init();

            _progressRepository.HasDailyTarget.SubscribeToInteractableUntilDestroy(_startPracticeButton);

            _windowsController.BindPopUpOpen(_startPracticeButton, PopUpType.WordPractice);

            _progressRepository.ProgressHistory.SubscribeUntilDestroy(this, static self => self.UpdateProgressText());
            _progressRepository.NewWordsDailyTarget
                .SubscribeUntilDestroy(this, static self => self.UpdateWordsGoalText());

            LocalizationController.Language.SubscribeUntilDestroy(this, static self =>
            {
                self.UpdateProgressText();
                self.UpdateWordsGoalText();
            });
        }

        private void UpdateProgressText()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var progress = _progressRepository.ProgressHistory.CurrentValue;
            var repeatableCount = progress.TryGetValue(today, out var dailyProgress)
                ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Review))
                : 0;

            _repetitionText.SetTextFormat(
                _localizationKeysDatabase.GetLocalization(LocalizationType.RepetitionGoal),
                repeatableCount);
        }

        private void UpdateWordsGoalText()
        {
            var wordsTarget = _progressRepository.NewWordsDailyTarget;

            var localization = _localizationKeysDatabase.GetLocalization(LocalizationType.LearnGoal);

            _learnGoalText.SetTextFormat(localization, wordsTarget);
        }
    }
}