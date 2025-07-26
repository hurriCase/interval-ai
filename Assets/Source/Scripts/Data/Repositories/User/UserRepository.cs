using System;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.User.Base;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IDisposable
    {
        public PersistentReactiveProperty<string> Nickname { get; }
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; }

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            UserIcon = new PersistentReactiveProperty<CachedSprite>(PersistentPropertyKeys.RepetitionByCooldownKey,
                new CachedSprite(defaultUserDataDatabase.Icon));

            Nickname = new PersistentReactiveProperty<string>(PersistentPropertyKeys.UserNameKey,
                defaultUserDataDatabase.Name);
        }

        public void Dispose()
        {
            Nickname.Dispose();
            UserIcon.Dispose();
        }
    }
}