using System;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours
{
    internal sealed class WordLearningBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnGoalText;
        [SerializeField] private TextMeshProUGUI _repetitionText;

        [SerializeField] private TextMeshProUGUI _dailyWordGoalText;

        [SerializeField] private ButtonComponent _minusButton;
        [SerializeField] private ButtonComponent _plusButton;

        internal void Init()
        {
            UpdateUI();

            ProgressRepository.Instance.ProgressEntry
                .Subscribe(this, static (_, behaviour) => behaviour.UpdateUI())
                .AddTo(this);

            _minusButton.Button.OnClickAsObservable()
                .Where(static _ => ProgressRepository.Instance.ProgressEntry.Value.DailyWordsGoal > 0)
                .Subscribe(this, static (_, behaviour) => behaviour.ModifyDailyGoal(-1))
                .AddTo(this);

            _plusButton.Button.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) => behaviour.ModifyDailyGoal(+1))
                .AddTo(this);
        }

        private void ModifyDailyGoal(int addAmount)
        {
            var value = ProgressRepository.Instance.ProgressEntry.Value;
            value.DailyWordsGoal += addAmount;
            ProgressRepository.Instance.ProgressEntry.Value = value;
        }

        private void UpdateUI()
        {
            var currentProgress = ProgressRepository.Instance.ProgressEntry.Value;

            _minusButton.Button.interactable = currentProgress.DailyWordsGoal > 0;
            _dailyWordGoalText.text = currentProgress.DailyWordsGoal.ToString();
            _learnGoalText.text =
                string.Format(LocalizationType.LearnGoal.GetLocalization(), currentProgress.DailyWordsGoal);

            var repeatableCount = currentProgress.ProgressHistory.TryGetValue(DateTime.Now, out var dailyProgress)
                ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.Repeatable))
                : 0;

            _repetitionText.text = string.Format(
                LocalizationType.RepetitionGoal.GetLocalization(),
                repeatableCount);
        }
    }
}