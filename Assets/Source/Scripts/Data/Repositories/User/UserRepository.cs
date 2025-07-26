using System;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.User.Base;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IDisposable
    {
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; }
        public PersistentReactiveProperty<string> Nickname { get; }

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            UserIcon = new PersistentReactiveProperty<CachedSprite>(PersistentPropertyKeys.UserIconKey,
                new CachedSprite(defaultUserDataDatabase.Icon));

            Nickname = new PersistentReactiveProperty<string>(PersistentPropertyKeys.NicknameKey,
                defaultUserDataDatabase.Name);
        }

        public void Dispose()
        {
            Nickname.Dispose();
            UserIcon.Dispose();
        }
    }
}