using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours
{
    internal sealed class UserWelcomeBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _welcomeText;
        [SerializeField] private LocalizationKey _welcomeKey;

        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        internal void Init()
        {
            _userRepository.Nickname.SubscribeToText(_welcomeKey, _welcomeText);
        }
    }
}