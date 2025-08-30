using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _startPracticeButton;

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

            _progressRepository.HasDailyTarget.SubscribeAndRegister(this,
                static (canReduce, self) => self._startPracticeButton.interactable = canReduce);

            _startPracticeButton.OnClickAsObservable()
                .Subscribe(_windowsController,
                    static (_, controller) => controller.OpenPopUpByType(PopUpType.WordPractice))
                .RegisterTo(destroyCancellationToken);

            _progressRepository.ProgressHistory.SubscribeAndRegister(this, static self => self.UpdateProgressText());
            _progressRepository.NewWordsDailyTarget
                .SubscribeAndRegister(this, static self => self.UpdateWordsGoalText());

            LocalizationController.Language.SubscribeAndRegister(this, static self =>
            {
                self.UpdateProgressText();
                self.UpdateWordsGoalText();
            });
        }

        private void UpdateProgressText()
        {
            var progress = _progressRepository.ProgressHistory.CurrentValue;
            var repeatableCount =
                progress.TryGetValue(DateTime.Now, out var dailyProgress)
                    ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Review))
                    : 0;

            _repetitionText.SetTextFormat(
                _localizationKeysDatabase.GetLocalization(LocalizationType.RepetitionGoal),
                repeatableCount);
        }

        private void UpdateWordsGoalText()
        {
            var wordsTarget = _progressRepository.NewWordsDailyTarget;

            var localization =
                _localizationKeysDatabase.GetLocalization(LocalizationType.LearnGoal);

            _learnGoalText.SetTextFormat(localization, wordsTarget);
        }
    }
}