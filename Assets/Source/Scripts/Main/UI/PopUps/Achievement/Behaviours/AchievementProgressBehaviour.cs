using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours
{
    internal sealed class AchievementProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnedWordsText;
        [SerializeField] private TextMeshProUGUI _bestStreakText;
        [SerializeField] private TextMeshProUGUI _currentStreakText;

        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _learnedWordsText.text =
                _progressRepository.TotalCountByState.CurrentValue[LearningState.Studied].ToString();

            _bestStreakText.text = _progressRepository.BestStreak.CurrentValue.ToString();
            _currentStreakText.text = _progressRepository.CurrentStreak.CurrentValue.ToString();
        }
    }
}