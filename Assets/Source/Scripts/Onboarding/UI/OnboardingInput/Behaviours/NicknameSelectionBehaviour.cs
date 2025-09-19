using Source.Scripts.Core.Repositories.User.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class NicknameSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private TextMeshProUGUI _placeholderText;

        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        internal override void Init()
        {
            _placeholderText.text = _userRepository.Nickname.CurrentValue;
        }

        internal override void HandleContinue()
        {
            _userRepository.SetNickname(_nicknameInputField.text);
        }
    }
}