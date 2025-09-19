using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Settings.Behaviours
{
    internal sealed class NicknameInputBehaviour : MonoBehaviour
    {
        [SerializeField] private InputFieldComponent _nicknameField;
        [SerializeField] private UserIconBehaviour _userIconBehaviour;

        private string _originalNickname;

        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        internal void Init()
        {
            _userIconBehaviour.Init();

            _userRepository.Nickname
                .SubscribeAndRegister(this, static (newName, self) => self._nicknameField.text = newName);

            _nicknameField.OnTextChanged
                .SubscribeAndRegister(this, static (newName, self) => self._userRepository.SetNickname(newName));
        }
    }
}