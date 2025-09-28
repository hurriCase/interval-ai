using System;
using System.Threading;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.User.Base;

namespace Source.Scripts.Core.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IRepository, IDisposable
    {
        public ReadOnlyReactiveProperty<string> Nickname => _nickname.Property;
        public ReadOnlyReactiveProperty<CachedSprite> UserIcon => _userIcon.Property;

        private readonly PersistentReactiveProperty<string> _nickname = new();
        private readonly PersistentReactiveProperty<CachedSprite> _userIcon = new();

        private readonly IDefaultUserDataConfig _defaultUserDataConfig;

        internal UserRepository(IDefaultUserDataConfig defaultUserDataConfig)
        {
            _defaultUserDataConfig = defaultUserDataConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                _userIcon.InitAsync(
                    PersistentKeys.UserIconKey,
                    token,
                    new CachedSprite(_defaultUserDataConfig.Icon)),

                _nickname.InitAsync(
                    PersistentKeys.NicknameKey,
                    token,
                    _defaultUserDataConfig.Name)
            };

            await UniTask.WhenAll(initTasks);
        }

        public void SetNickname(string nickname)
        {
            if (string.IsNullOrEmpty(nickname) is false)
                _nickname.Value = nickname;
        }

        public void Dispose()
        {
            _nickname.Dispose();
            _userIcon.Dispose();
        }
    }
}