using System;
using System.Threading;
using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.User.Base;

namespace Source.Scripts.Core.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IRepository, IDisposable
    {
        public PersistentReactiveProperty<string> Nickname { get; } = new();
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; } = new();

        private readonly IDefaultUserDataConfig _defaultUserDataConfig;

        internal UserRepository(IDefaultUserDataConfig defaultUserDataConfig)
        {
            _defaultUserDataConfig = defaultUserDataConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                UserIcon.InitAsync(
                    PersistentKeys.UserIconKey,
                    token,
                    new CachedSprite(_defaultUserDataConfig.Icon)),

                Nickname.InitAsync(
                    PersistentKeys.NicknameKey,
                    token,
                    _defaultUserDataConfig.Name)
            };

            await UniTask.WhenAll(initTasks);
        }

        public void Dispose()
        {
            Nickname.Dispose();
            UserIcon.Dispose();
        }
    }
}