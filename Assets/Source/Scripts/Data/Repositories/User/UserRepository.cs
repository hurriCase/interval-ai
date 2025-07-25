using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.CooldownSystem;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IDisposable
    {
        public PersistentReactiveProperty<string> Nickname { get; } = new(PersistentPropertyKeys.UserNameKey,
            _defaultUserDataDatabase.Name);

        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } =
            new(PersistentPropertyKeys.CurrentCultureKey, CultureInfo.CurrentCulture);

        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } =
            new(PersistentPropertyKeys.LearningDirectionKey, LearningDirectionType.LearningToNative);

        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } =
            new(PersistentPropertyKeys.RepetitionByCooldownKey, _defaultUserDataDatabase.DefaultCooldowns);

        public PersistentReactiveProperty<Sprite> UserIcon { get; } =
            new(PersistentPropertyKeys.RepetitionByCooldownKey, _defaultUserDataDatabase.Icon);

        public PersistentReactiveProperty<LanguageLevel> UserLevel { get; } = new(PersistentPropertyKeys.UserLevelKey);
        public PersistentReactiveProperty<bool> IsCompleteOnboarding { get; } =
            new(PersistentPropertyKeys.IsCompleteOnboardingKey);
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; } =
            new(PersistentPropertyKeys.LanguageByTypeKey);

        private static IDefaultUserDataDatabase _defaultUserDataDatabase;

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            _defaultUserDataDatabase = defaultUserDataDatabase;
        }

        public void Dispose()
        {
            Nickname?.Dispose();
            CurrentCulture?.Dispose();
        }
    }
}