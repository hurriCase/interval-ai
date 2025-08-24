using CustomUtils.Runtime.AddressableSystem;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Core.Repositories.User.Base
{
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CachedSprite> UserIcon { get; }
    }
}