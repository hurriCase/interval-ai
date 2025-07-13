using System;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _startPracticeButton;

        [SerializeField] private TextMeshProUGUI _learnGoalText;
        [SerializeField] private TextMeshProUGUI _repetitionText;

        [SerializeField] private TextMeshProUGUI _dailyWordGoalText;

        [SerializeField] private ButtonComponent _minusButton;
        [SerializeField] private ButtonComponent _plusButton;

        internal void Init()
        {
            UpdateUI();

            _startPracticeButton.OnClickAsObservable().Subscribe(static _ =>
                WindowsController.Instance.OpenPopUpByType(PopUpType.WordPractice));

            ProgressRepository.Instance.ProgressHistory
                .Subscribe(this, static (_, behaviour) => behaviour.UpdateUI())
                .AddTo(this);

            var dailyWordsGoal = ProgressRepository.Instance.DailyWordsGoal;
            _minusButton.OnClickAsObservable()
                .Where(dailyWordsGoal, (_, goal) => goal.Value > 0)
                .Subscribe(dailyWordsGoal, static (_, goal) => goal.Value--)
                .AddTo(this);

            _plusButton.OnClickAsObservable()
                .Subscribe(dailyWordsGoal, static (_, goal) => goal.Value++)
                .AddTo(this);
        }

        private void UpdateUI()
        {
            var repository = ProgressRepository.Instance;
            _minusButton.interactable = repository.DailyWordsGoal.Value > 0;
            _dailyWordGoalText.text = repository.DailyWordsGoal.Value.ToString();
            _learnGoalText.text =
                string.Format(LocalizationType.LearnGoal.GetLocalization(), repository.DailyWordsGoal.Value);

            var repeatableCount =
                repository.ProgressHistory.Value.TryGetValue(DateTime.Now, out var dailyProgress)
                    ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Repeatable))
                    : 0;

            _repetitionText.text = string.Format(
                LocalizationType.RepetitionGoal.GetLocalization(),
                repeatableCount);
        }
    }
}