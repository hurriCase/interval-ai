using R3;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.UI.Selectables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Windows.Screens.Settings.Behaviours
{
    internal sealed class UserBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private TMP_InputField _nicknameField;
        [SerializeField] private Image _userIcon;

        private string _originalNickname;

        internal void Init()
        {
            _editButton.OnClickAsObservable()
                .Subscribe(this, static (_, screen) => screen.StartEditing())
                .RegisterTo(destroyCancellationToken);

            _nicknameField.onEndEdit.AddListener(FinishEditing);

            _nicknameField.text = UserRepository.Instance.Nickname.Value;
            _userIcon.sprite = UserRepository.Instance.UserIcon.Value;

            UserRepository.Instance.Nickname
                .AsObservable()
                .Where(this, static (_, screen) => screen._nicknameField.interactable is false)
                .Subscribe(this, static (userName, screen) => screen._nicknameField.text = userName)
                .RegisterTo(destroyCancellationToken);

            UserRepository.Instance.UserIcon
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

            UserRepository.Instance.Nickname.Value =
                string.IsNullOrEmpty(newNickname) ? _originalNickname : newNickname;
        }

        private void OnDestroy()
        {
            _nicknameField.onEndEdit.RemoveListener(FinishEditing);
        }
    }
}