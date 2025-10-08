using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.LearningWords
{
    internal sealed class MainScreen : ScreenBase
    {
        [SerializeField] private DailyProgressBehaviour _dailyProgressBehaviour;
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private WordLearningBehaviour _wordLearningBehaviour;
        [SerializeField] private AchievementsBehaviour _achievementsBehaviour;
        [SerializeField] private UserIconBehaviour _userIconBehaviour;
        [SerializeField] private UserWelcomeBehaviour _userWelcomeBehaviour;

        internal override void Init()
        {
            _dailyProgressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();
            _userIconBehaviour.Init();
            _userWelcomeBehaviour.Init();
        }
    }
}