using Client.Scripts.Core.MVC.Base;
using UnityEngine;

namespace Client.Scripts.Core.MVC.User
{
    internal sealed class UserView : MonoBehaviour, IView<UserModel>
    {
        [SerializeField] private Transform _userContainer;
        [SerializeField] private GameObject _userItemPrefab;

        private UserController _controller;

        internal void Init(UserController controller)
        {
            _controller = controller;
        }

        public void UpdateView(UserModel model)
        {
            var item = Instantiate(_userItemPrefab, _userContainer);
            if (item.TryGetComponent<UserItemView>(out var userItem) is false)
                return;

            userItem.Init(model, _controller);
        }

        public void ClearView()
        {
            foreach (Transform child in _userContainer)
                Destroy(child.gameObject);
        }
    }
}