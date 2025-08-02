using System;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.User.Base;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IDisposable
    {
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; }
        public PersistentReactiveProperty<string> Nickname { get; }

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            UserIcon = new PersistentReactiveProperty<CachedSprite>(PersistentKeys.UserIconKey,
                new CachedSprite(defaultUserDataDatabase.Icon));

            Nickname = new PersistentReactiveProperty<string>(PersistentKeys.NicknameKey,
                defaultUserDataDatabase.Name);
        }

        public void Dispose()
        {
            Nickname.Dispose();
            UserIcon.Dispose();
        }
    }
}