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
        [SerializeField] private ProgressBehaviour _progressBehaviour;
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private WordLearningBehaviour _wordLearningBehaviour;
        [SerializeField] private AchievementsBehaviour _achievementsBehaviour;

        [SerializeField] private TextMeshProUGUI _welcomeText;

        internal override void Init()
        {
            _progressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();

            _welcomeText.text =
                string.Format(LocalizationType.UserWelcome.GetLocalization(), UserRepository.Instance.UserName.Value);
        }
    }
}