using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.User.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class UserIconBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _userIcon;

        private IAddressablesLoader _addressablesLoader;
        private IUserRepository _userRepository;

        [Inject]
        internal void Inject(IAddressablesLoader addressablesLoader, IUserRepository userRepository)
        {
            _addressablesLoader = addressablesLoader;
            _userRepository = userRepository;
        }

        internal void Init()
        {
            _userRepository.UserIcon
                .SubscribeAndRegister(this, static (cachedSprite, self)
                    => self._addressablesLoader.AssignImageAsync(
                        self._userIcon, cachedSprite, self.destroyCancellationToken).Forget());
        }
    }
}