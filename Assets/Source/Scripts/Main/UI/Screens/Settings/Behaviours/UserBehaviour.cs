using R3;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.UI.Selectables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Screens.Settings.Behaviours
{
    internal sealed class UserBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private TMP_InputField _nicknameField;
        [SerializeField] private Image _userIcon;

        [Inject] private IUserRepository _userRepository;

        private string _originalNickname;

        internal void Init()
        {
            _editButton.OnClickAsObservable()
                .Subscribe(this, static (_, screen) => screen.StartEditing())
                .RegisterTo(destroyCancellationToken);

            _nicknameField.onEndEdit.AddListener(FinishEditing);

            _nicknameField.text = _userRepository.Nickname.Value;
            _userIcon.sprite = _userRepository.UserIcon.Value;

            _userRepository.Nickname
                .AsObservable()
                .Where(this, static (_, screen) => screen._nicknameField.interactable is false)
                .Subscribe(this, static (userName, screen) => screen._nicknameField.text = userName)
                .RegisterTo(destroyCancellationToken);

            _userRepository.UserIcon
                .Subscribe(this, static (icon, screen) => screen._userIcon.sprite = icon)
                .RegisterTo(destroyCancellationToken);
        }

        private void StartEditing()
        {
            _originalNickname = _nicknameField.text;
            _nicknameField.interactable = true;
            _nicknameField.ActivateInputField();
        }

        private void FinishEditing(string newNickname)
        {
            _nicknameField.interactable = false;

            _userRepository.Nickname.Value =
                string.IsNullOrEmpty(newNickname) ? _originalNickname : newNickname;
        }

        private void OnDestroy()
        {
            _nicknameField.onEndEdit.RemoveListener(FinishEditing);
        }
    }
}