using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core.MVC.User
{
    internal class UserItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _passwordText;
        [SerializeField] private TextMeshProUGUI _emailText;
        [SerializeField] private Button _deleteButton;

        private UserController _controller;
        private UserModel _model;

        public void Init(UserModel model, UserController controller)
        {
            _model = model;
            _controller = controller;

            UpdateUI();
            SetupListeners();
        }

        private void OnDestroy() => _deleteButton.onClick.RemoveAllListeners();

        private void UpdateUI()
        {
            _userNameText.text = _model.UserName;
            _passwordText.text = _model.Password;
            _emailText.text = _model.Email;
        }

        private void SetupListeners() => _deleteButton.onClick.AddListener(DeleteEntry);

        private async void DeleteEntry()
        {
            try
            {
                await _controller.DeleteEntry(_model.Data.Id);
            }
            catch (Exception e)
            {
                Debug.LogError($"[UserItemView::DeleteEntry] Entity deletion failed: {e.Message}");
            }
        }
    }
}