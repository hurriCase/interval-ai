using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.Settings.Behaviours
{
    internal sealed class UserBehaviour : MonoBehaviour
    {
        [SerializeField] private InputFieldComponent _nicknameField;
        [SerializeField] private Image _userIcon;

        [Inject] private IUserRepository _userRepository;
        [Inject] private IAddressablesLoader _addressablesLoader;

        private string _originalNickname;

        internal void Init()
        {
            SetUserIcon(_userRepository.UserIcon.Value.AssetGUID).Forget();

            _userRepository.Nickname
                .Subscribe(this, static (newName, self) => self._nicknameField.text = newName)
                .RegisterTo(destroyCancellationToken);

            _userRepository.UserIcon
                .Subscribe(this, static (cachedSprite, screen)
                    => screen.SetUserIcon(cachedSprite.AssetGUID).Forget())
                .RegisterTo(destroyCancellationToken);

            _nicknameField.CurrentTextSubjectObservable
                .Subscribe(this, static (newName, self) => self._userRepository.Nickname.Value = newName)
                .RegisterTo(destroyCancellationToken);
        }

        private async UniTask SetUserIcon(string assetGUID)
        {
            _userIcon.sprite = await _addressablesLoader.LoadAsync<Sprite>(assetGUID, destroyCancellationToken);
        }
    }
}