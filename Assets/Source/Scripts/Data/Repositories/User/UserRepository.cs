using System;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using R3;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : Singleton<UserRepository>, IDisposable
    {
        internal PersistentReactiveProperty<UserEntry> UserEntry { get; } =
            new(PersistentPropertyKeys.WordEntryKey, new UserEntry("user", CultureInfo.CurrentCulture));

        internal Observable<CultureInfo> CultureChanged =>
            UserEntry.AsObservable()
                .Select(entry => entry.CurrentCulture)
                .DistinctUntilChanged();

        internal CultureInfo CurrentCulture => UserEntry.Value.CurrentCulture;

        public void Dispose()
        {
            UserEntry.Dispose();
        }
    }
}