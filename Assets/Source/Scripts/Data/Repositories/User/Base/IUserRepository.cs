using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Data.Repositories.User.Base
{
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CachedSprite> UserIcon { get; }
    }
}