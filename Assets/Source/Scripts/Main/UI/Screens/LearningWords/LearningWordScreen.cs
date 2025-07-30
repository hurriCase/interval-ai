using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Main.Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords.Behaviours;
using Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords
{
    internal sealed class LearningWordScreen : ScreenBase
    {
        [SerializeField] private DailyProgressBehaviour _dailyProgressBehaviour;
        [SerializeField] private CategoryPreviewBehaviour _categoryPreviewBehaviour;
        [SerializeField] private WordLearningBehaviour _wordLearningBehaviour;
        [SerializeField] private AchievementsBehaviour _achievementsBehaviour;

        [SerializeField] private TextMeshProUGUI _welcomeText;

        [Inject] private IUserRepository _userRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

        internal override void Init()
        {
            _dailyProgressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();

            _userRepository.Nickname
                .Subscribe(this,
                    static (nickname, screen) =>
                    {
                        var localization =
                            screen._localizationKeysDatabase.GetLocalization(LocalizationType.UserWelcome);

                        screen._welcomeText.text = string.Format(localization, nickname);
                    })
                .RegisterTo(destroyCancellationToken);
        }
    }
}