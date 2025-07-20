using R3;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords
{
    internal sealed class LearningWordsScreen : ScreenBase
    {
        [SerializeField] private DailyProgressBehaviour _dailyProgressBehaviour;
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private WordLearningBehaviour _wordLearningBehaviour;
        [SerializeField] private AchievementsBehaviour _achievementsBehaviour;

        [SerializeField] private TextMeshProUGUI _welcomeText;

        internal override void Init()
        {
            _dailyProgressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();

            UserRepository.Instance.Nickname
                .Subscribe(this,
                    static (nickname, screen) => screen._welcomeText.text =
                        string.Format(LocalizationType.UserWelcome.GetLocalization(), nickname))
                .RegisterTo(destroyCancellationToken);
        }
    }
}