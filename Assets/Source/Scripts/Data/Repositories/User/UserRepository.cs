using System;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : Singleton<UserRepository>, IDisposable
    {
        internal PersistentReactiveProperty<UserEntry> UserEntry { get; } =
            new(PersistentPropertyKeys.WordEntryKey, new UserEntry("user", CultureInfo.CurrentCulture));

        internal CultureInfo CurrentCulture => UserEntry.Value.CurrentCulture;

        public void Dispose()
        {
            UserEntry.Dispose();
        }
    }
}