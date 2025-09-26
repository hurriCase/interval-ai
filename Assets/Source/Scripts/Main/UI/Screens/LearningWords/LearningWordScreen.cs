using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.CategoryPreview;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using Source.Scripts.Main.UI.Shared;
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
        [SerializeField] private UserIconBehaviour _userIconBehaviour;

        [SerializeField] private TextMeshProUGUI _welcomeText;

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IUserRepository userRepository, ILocalizationKeysDatabase localizationKeysDatabase)
        {
            _userRepository = userRepository;
            _localizationKeysDatabase = localizationKeysDatabase;
        }

        internal override void Init()
        {
            _dailyProgressBehaviour.Init();
            _categoryPreviewBehaviour.Init();
            _wordLearningBehaviour.Init();
            _achievementsBehaviour.Init();
            _userIconBehaviour.Init();

            _userRepository.Nickname.SubscribeUntilDestroy(this, static self => self.UpdateUserWelcome());
            LocalizationController.Language.SubscribeUntilDestroy(this, static self => self.UpdateUserWelcome());
        }

        private void UpdateUserWelcome()
        {
            var localization = _localizationKeysDatabase.GetLocalization(LocalizationType.UserWelcome);

            _welcomeText.SetTextFormat(localization, _userRepository.Nickname.CurrentValue);
        }
    }
}