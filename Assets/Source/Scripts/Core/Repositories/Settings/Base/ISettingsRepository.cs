using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme.Base;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface ISettingsRepository
    {
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        PersistentReactiveProperty<EnumArray<LanguageType, ReactiveProperty<SystemLanguage>>> LanguageByType { get; }
        PersistentReactiveProperty<SystemLanguage> SystemLanguage { get; }
        PersistentReactiveProperty<LanguageType> FirstShowPractice { get; }
        PersistentReactiveProperty<LanguageType> CardLearnPractice { get; }
        PersistentReactiveProperty<LanguageType> CardReviewPractice { get; }
        PersistentReactiveProperty<int> DailyGoal { get; }
        PersistentReactiveProperty<ThemeType> ThemeType { get; }
        PersistentReactiveProperty<WordReviewSourceType> WordReviewSourceType { get; }
        PersistentReactiveProperty<bool> IsSendNotifications { get; }
        PersistentReactiveProperty<bool> IsShowTranscription { get; }
        PersistentReactiveProperty<bool> IsSwipeEnabled { get; }
        void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType);
    }
}