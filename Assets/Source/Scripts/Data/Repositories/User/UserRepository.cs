using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Vocabulary;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : Singleton<UserRepository>, IDisposable
    {
        public PersistentReactiveProperty<string> UserName { get; } = new(PersistentPropertyKeys.UserNameKey, "user");

        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } =
            new(PersistentPropertyKeys.CurrentCultureKey, CultureInfo.CurrentCulture);

        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } =
            new(PersistentPropertyKeys.LearningDirectionKey, LearningDirectionType.LearningToNative);

        public PersistentReactiveProperty<List<CooldownData>> RepetitionByCooldown { get; } =
            new(PersistentPropertyKeys.RepetitionByCooldownKey, DefaultCooldownsDatabase.Instance.DefaultCooldowns);

        public void Dispose()
        {
            UserName?.Dispose();
            CurrentCulture?.Dispose();
        }
    }
}