using System;
using R3;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
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
                .Subscribe(this, static (progress, behaviour) =>
                {
                    var repeatableCount =
                        progress.TryGetValue(DateTime.Now, out var dailyProgress)
                            ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Repeatable))
                            : 0;

                    behaviour._repetitionText.text = string.Format(
                        behaviour._localizationKeysDatabase.GetLocalization(LocalizationType.RepetitionGoal),
                        repeatableCount);
                })
                .RegisterTo(destroyCancellationToken);

            _progressRepository.NewWordsDailyTarget
                .Subscribe(this, static (wordsTarget, behaviour) =>
                {
                    var localization =
                        behaviour._localizationKeysDatabase.GetLocalization(LocalizationType.LearnGoal);

                    behaviour._learnGoalText.text = string.Format(localization, wordsTarget);
                })
                .RegisterTo(destroyCancellationToken);
        }
    }
}