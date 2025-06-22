using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords
{
    internal sealed class LearningWordsScreen : ScreenBase
    {
        [SerializeField] private ProgressBehaviour _progressBehaviour;
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private WordLearningBehaviour _wordLearningBehaviour;
        [SerializeField] private AchievementsBehaviour _achievementsBehaviour;

        internal override void Init()
        {
            _progressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();
        }
    }
}