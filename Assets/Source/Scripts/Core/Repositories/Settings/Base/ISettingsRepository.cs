using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface ISettingsRepository
    {
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        PersistentReactiveProperty<EnumArray<LanguageType, SystemLanguage>> LanguageByType { get; }
        PersistentReactiveProperty<int> DailyGoal { get; }
        PersistentReactiveProperty<ThemeType> CurrentTheme { get; }
        PersistentReactiveProperty<bool> IsSendNotifications { get; }
        PersistentReactiveProperty<bool> IsShowTranscription { get; }
        PersistentReactiveProperty<bool> IsSwipeEnabled { get; }
        void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType);
    }
}