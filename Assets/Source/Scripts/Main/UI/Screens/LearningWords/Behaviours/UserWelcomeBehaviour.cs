using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.User.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class UserWelcomeBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _welcomeText;

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IUserRepository userRepository, ILocalizationKeysDatabase localizationKeysDatabase)
        {
            _userRepository = userRepository;
            _localizationKeysDatabase = localizationKeysDatabase;
        }

        internal void Init()
        {
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