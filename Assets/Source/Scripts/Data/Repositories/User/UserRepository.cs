using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class UserRepository : IUserRepository, IDisposable
    {
        public PersistentReactiveProperty<string> Nickname { get; }
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        public PersistentReactiveProperty<CachedSprite> UserIcon { get; }
        public PersistentReactiveProperty<LanguageLevel> UserLevel { get; }
        public PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }

        internal UserRepository(IDefaultUserDataDatabase defaultUserDataDatabase)
        {
            CurrentCulture = new PersistentReactiveProperty<CultureInfo>(PersistentPropertyKeys.CurrentCultureKey,
                CultureInfo.CurrentCulture);

            LearningDirection =
                new PersistentReactiveProperty<LearningDirectionType>(PersistentPropertyKeys.LearningDirectionKey,
                    LearningDirectionType.LearningToNative);

            RepetitionByCooldown = new PersistentReactiveProperty<List<CooldownByDate>>(
                PersistentPropertyKeys.RepetitionByCooldownKey, defaultUserDataDatabase.DefaultCooldowns);

            UserIcon = new PersistentReactiveProperty<CachedSprite>(PersistentPropertyKeys.RepetitionByCooldownKey,
                new CachedSprite(defaultUserDataDatabase.Icon));

            UserLevel = new PersistentReactiveProperty<LanguageLevel>(PersistentPropertyKeys.UserLevelKey);
            IsCompleteOnboarding = new PersistentReactiveProperty<bool>(PersistentPropertyKeys.IsCompleteOnboardingKey);
            LanguageByType = new PersistentReactiveProperty<EnumArray<LanguageType, Language>>(PersistentPropertyKeys
                .LanguageByTypeKey);

            Nickname = new PersistentReactiveProperty<string>(PersistentPropertyKeys.UserNameKey,
                defaultUserDataDatabase.Name);
        }

        public void Dispose()
        {
            Nickname?.Dispose();
            CurrentCulture?.Dispose();
            LearningDirection?.Dispose();
            RepetitionByCooldown?.Dispose();
            UserIcon?.Dispose();
            UserLevel?.Dispose();
            IsCompleteOnboarding?.Dispose();
            LanguageByType?.Dispose();
        }
    }
}