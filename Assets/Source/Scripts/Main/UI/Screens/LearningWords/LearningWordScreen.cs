using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords
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

            _userRepository.Nickname.SubscribeAndRegister(this, static self => self.UpdateUserWelcome());
            LocalizationController.Language.SubscribeAndRegister(this, static self => self.UpdateUserWelcome());
        }

        private void UpdateUserWelcome()
        {
            var localization = _localizationKeysDatabase.GetLocalization(LocalizationType.UserWelcome);

            _welcomeText.SetTextFormat(localization, _userRepository.Nickname.CurrentValue);
        }
    }
}