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

        [Inject] private IUserRepository _userRepository;

        internal override void Init()
        {
            _placeholderText.text = _userRepository.Nickname.CurrentValue;
        }

        internal override void OnContinue()
        {
            _userRepository.SetNickname(_nicknameInputField.text);
        }
    }
}