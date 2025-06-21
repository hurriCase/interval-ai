using R3;
using Source.Scripts.Data;
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

            UserData.Instance.ProgressEntry
                .Subscribe(this, static (_, behaviour) => behaviour.UpdateUI())
                .AddTo(this);

            _minusButton.Button.OnClickAsObservable()
                .Where(static _ => UserData.Instance.ProgressEntry.Value.DailyWordGoal > 0)
                .Subscribe(this, static (_, behaviour) => behaviour.ModifyDailyGoal(-1))
                .AddTo(this);

            _plusButton.Button.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) => behaviour.ModifyDailyGoal(+1))
                .AddTo(this);
        }

        private void ModifyDailyGoal(int addAmount)
        {
            var value = UserData.Instance.ProgressEntry.Value;
            value.DailyWordGoal += addAmount;
            UserData.Instance.ProgressEntry.Value = value;
        }

        private void UpdateUI()
        {
            _minusButton.Button.interactable = UserData.Instance.ProgressEntry.Value.DailyWordGoal > 0;

            var currentProgress = UserData.Instance.ProgressEntry.Value;
            _dailyWordGoalText.text = currentProgress.DailyWordGoal.ToString();
            _learnGoalText.text =
                string.Format(LocalizationType.LearnGoal.GetLocalization(), currentProgress.DailyWordGoal);
            _repetitionText.text =
                string.Format(LocalizationType.RepetitionGoal.GetLocalization(), currentProgress.RepeatingWordsCount);
        }
    }
}