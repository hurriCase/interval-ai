using System;
using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Settings
{
    internal sealed class SettingsRepository : IDisposable, ISettingsRepository
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        public PersistentReactiveProperty<int> DailyGoal { get; }
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }

        internal SettingsRepository(IDefaultSettingsDatabase defaultSettingsDatabase)
        {
            LanguageLevel = new PersistentReactiveProperty<LanguageLevel>(PersistentKeys.LanguageLevelKey);

            DailyGoal = new PersistentReactiveProperty<int>(PersistentKeys.DailyGoalKey,
                defaultSettingsDatabase.DailyGoal);

            CurrentCulture = new PersistentReactiveProperty<CultureInfo>(PersistentKeys.CurrentCultureKey,
                CultureInfo.CurrentCulture);

            RepetitionByCooldown = new PersistentReactiveProperty<List<CooldownByDate>>(
                PersistentKeys.RepetitionByCooldownKey, defaultSettingsDatabase.Cooldowns);

            LanguageByType = new PersistentReactiveProperty<EnumArray<LanguageType, Language>>(PersistentKeys
                .LanguageByTypeKey);

            LearningDirection =
                new PersistentReactiveProperty<LearningDirectionType>(PersistentKeys.LearningDirectionKey,
                    LearningDirectionType.LearningToNative);
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