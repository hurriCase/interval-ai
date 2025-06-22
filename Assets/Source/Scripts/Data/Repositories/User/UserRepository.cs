using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : Singleton<UserRepository>
    {
        internal PersistentReactiveProperty<UserEntry> UserEntry { get; } =
            new(PersistentPropertyKeys.WordEntryKey, new UserEntry("user"));
    }
}