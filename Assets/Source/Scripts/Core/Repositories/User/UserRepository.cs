using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.User.Base;
using Source.Scripts.Core.Sprites;

namespace Source.Scripts.Core.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IRepository
    {
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; } = new();
        public PersistentReactiveProperty<string> Nickname { get; } = new();

        private readonly IDefaultUserDataConfig _defaultUserDataConfig;

        internal UserRepository(IDefaultUserDataConfig defaultUserDataConfig)
        {
            _defaultUserDataConfig = defaultUserDataConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                UserIcon.InitAsync(
                    PersistentKeys.UserIconKey,
                    cancellationToken,
                    new CachedSprite(_defaultUserDataConfig.Icon)),

                Nickname.InitAsync(
                    PersistentKeys.NicknameKey,
                    cancellationToken,
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