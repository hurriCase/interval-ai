using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Others;

namespace Source.Scripts.Core.DI.Repositories.User.Base
{
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CachedSprite> UserIcon { get; }
    }
}