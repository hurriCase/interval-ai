using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
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

        [Inject] private IWindowsController _windowsController;
        [Inject] private IProgressRepository _progressRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal void Init()
        {
            _plusMinusBehaviour.Init();

            _startPracticeButton.OnClickAsObservable()
                .Subscribe(_windowsController,
                    static (_, controller) => controller.OpenPopUpByType(PopUpType.WordPractice))
                .RegisterTo(destroyCancellationToken);

            _progressRepository.ProgressHistory
                .SubscribeAndRegister(this, static (progress, self) => self.UpdateProgressText(progress));

            _progressRepository.NewWordsDailyTarget
                .SubscribeAndRegister(this, static (wordsTarget, self) => self.UpdateWordsGoalText(wordsTarget));
        }

        private void UpdateProgressText(Dictionary<DateTime, DailyProgress> progress)
        {
            var repeatableCount =
                progress.TryGetValue(DateTime.Now, out var dailyProgress)
                    ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Review))
                    : 0;

            _repetitionText.SetTextFormat(
                _localizationKeysDatabase.GetLocalization(LocalizationType.RepetitionGoal),
                repeatableCount);
        }

        private void UpdateWordsGoalText(int wordsTarget)
        {
            var localization =
                _localizationKeysDatabase.GetLocalization(LocalizationType.LearnGoal);

            _learnGoalText.SetTextFormat(localization, wordsTarget);
        }
    }
}