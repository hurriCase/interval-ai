using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.User.Base;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IRepository
    {
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; } = new();
        public PersistentReactiveProperty<string> Nickname { get; } = new();

        private readonly IDefaultUserDataDatabase _defaultUserDataDatabase;

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            _defaultUserDataDatabase = defaultUserDataDatabase;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                UserIcon.InitAsync(
                    PersistentKeys.UserIconKey,
                    cancellationToken,
                    new CachedSprite(_defaultUserDataDatabase.Icon)),

                Nickname.InitAsync(
                    PersistentKeys.NicknameKey,
                    cancellationToken,
                    _defaultUserDataDatabase.Name)
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