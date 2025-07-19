using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Singletons;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Vocabulary;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : Singleton<UserRepository>, IDisposable
    {
        public PersistentReactiveProperty<string> Nickname { get; } = new(PersistentPropertyKeys.UserNameKey,
            DefaultUserDataDatabase.Instance.Name);

        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } =
            new(PersistentPropertyKeys.CurrentCultureKey, CultureInfo.CurrentCulture);

        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } =
            new(PersistentPropertyKeys.LearningDirectionKey, LearningDirectionType.LearningToNative);

        public PersistentReactiveProperty<List<CooldownData>> RepetitionByCooldown { get; } =
            new(PersistentPropertyKeys.RepetitionByCooldownKey, DefaultUserDataDatabase.Instance.DefaultCooldowns);

        public PersistentReactiveProperty<Sprite> UserIcon { get; } =
            new(PersistentPropertyKeys.RepetitionByCooldownKey, DefaultUserDataDatabase.Instance.Icon);

        public void Dispose()
        {
            Nickname?.Dispose();
            CurrentCulture?.Dispose();
        }
    }
}