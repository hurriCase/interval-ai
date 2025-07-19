using System;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Shared;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _startPracticeButton;

        [SerializeField] private TextMeshProUGUI _learnGoalText;
        [SerializeField] private TextMeshProUGUI _repetitionText;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        internal void Init()
        {
            _plusMinusBehaviour.Init();

            _startPracticeButton.OnClickAsObservable().Subscribe(static _ =>
                WindowsController.Instance.OpenPopUpByType(PopUpType.WordPractice));

            ProgressRepository.Instance.ProgressHistory
                .Subscribe(this, static (progress, behaviour) =>
                {
                    var repeatableCount =
                        progress.TryGetValue(DateTime.Now, out var dailyProgress)
                            ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Repeatable))
                            : 0;

                    behaviour._repetitionText.text = string.Format(
                        LocalizationType.RepetitionGoal.GetLocalization(),
                        repeatableCount);
                })
                .RegisterTo(destroyCancellationToken);

            ProgressRepository.Instance.DailyWordsGoal
                .Subscribe(this, static (goal, behaviour) =>
                    behaviour._learnGoalText.text = string.Format(LocalizationType.LearnGoal.GetLocalization(), goal))
                .RegisterTo(destroyCancellationToken);
        }
    }
}