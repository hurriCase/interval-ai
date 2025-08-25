using CustomUtils.Runtime.AddressableSystem;
using R3;

namespace Source.Scripts.Core.Repositories.User.Base
{
    internal interface IUserRepository
    {
        ReadOnlyReactiveProperty<string> Nickname { get; }
        ReadOnlyReactiveProperty<CachedSprite> UserIcon { get; }
        void SetNickname(string nickname);
    }
}