using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Settings
{
    internal sealed class SettingsRepository : IDisposable, ISettingsRepository
    {
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }
        public PersistentReactiveProperty<int> DailyGoal { get; }

        internal SettingsRepository(IDefaultSettingsDatabase defaultSettingsDatabase)
        {
            CurrentCulture = new PersistentReactiveProperty<CultureInfo>(PersistentPropertyKeys.CurrentCultureKey,
                CultureInfo.CurrentCulture);

            LearningDirection =
                new PersistentReactiveProperty<LearningDirectionType>(PersistentPropertyKeys.LearningDirectionKey,
                    LearningDirectionType.LearningToNative);

            RepetitionByCooldown = new PersistentReactiveProperty<List<CooldownByDate>>(
                PersistentPropertyKeys.RepetitionByCooldownKey, defaultSettingsDatabase.DefaultCooldowns);

            LanguageLevel = new PersistentReactiveProperty<LanguageLevel>(PersistentPropertyKeys.UserLevelKey);

            LanguageByType = new PersistentReactiveProperty<EnumArray<LanguageType, Language>>(PersistentPropertyKeys
                .LanguageByTypeKey);

            DailyGoal = new PersistentReactiveProperty<int>(PersistentPropertyKeys.DailyGoalKey);
        }

        public void Dispose()
        {
            CurrentCulture.Dispose();
            LearningDirection.Dispose();
            RepetitionByCooldown.Dispose();
            LanguageLevel.Dispose();
            LanguageByType.Dispose();
            DailyGoal.Dispose();
        }
    }
}