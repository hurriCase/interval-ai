using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.User.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Setup.Behaviours
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

            _nicknameInputField.onEndEdit.AsObservable().SubscribeUntilDestroy(this,
                static (text, self) => self._userRepository.SetNickname(text));
        }
    }
}