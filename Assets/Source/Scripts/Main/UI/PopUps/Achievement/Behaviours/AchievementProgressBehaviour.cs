using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours
{
    internal sealed class AchievementProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnedWordsText;
        [SerializeField] private TextMeshProUGUI _bestStreakText;
        [SerializeField] private TextMeshProUGUI _currentStreakText;

        [Inject] private IProgressRepository _progressRepository;

        internal void Init()
        {
            _learnedWordsText.text = _progressRepository.TotalCountByState.Value[LearningState.Studied].ToString();
            _bestStreakText.text = _progressRepository.BestStreak.Value.ToString();
            _currentStreakText.text = _progressRepository.CurrentStreak.Value.ToString();
        }
    }
}