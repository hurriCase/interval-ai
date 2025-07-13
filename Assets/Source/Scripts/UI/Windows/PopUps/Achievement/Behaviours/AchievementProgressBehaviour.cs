using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours
{
    internal sealed class AchievementProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnedWordsText;
        [SerializeField] private TextMeshProUGUI _bestStreakText;
        [SerializeField] private TextMeshProUGUI _currentStreakText;

        internal void Init()
        {
            var repository = ProgressRepository.Instance;
            _learnedWordsText.text = repository.TotalCountByState.Value[LearningState.Studied].ToString();
            _bestStreakText.text = repository.BestStreak.Value.ToString();
            _currentStreakText.text = repository.CurrentStreak.Value.ToString();
        }
    }
}