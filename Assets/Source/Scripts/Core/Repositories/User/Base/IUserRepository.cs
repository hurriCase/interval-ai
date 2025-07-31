using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Others;

namespace Source.Scripts.Core.Repositories.User.Base
{
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CachedSprite> UserIcon { get; }
    }
}