using Source.Scripts.Core.Repositories.User.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours
{
    internal sealed class NicknameSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private TextMeshProUGUI _placeholderText;

        [Inject] private IUserRepository _userRepository;

        internal override void Init()
        {
            _placeholderText.text = _userRepository.Nickname.Value;
        }

        internal override void OnContinue()
        {
            if (string.IsNullOrEmpty(_nicknameInputField.text))
            {
                _userRepository.Nickname.Value = _placeholderText.text;
                return;
            }

            _userRepository.Nickname.Value = _nicknameInputField.text;
        }
    }
}