using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Words.Base;
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
        void SetLanguage(SystemLanguage newLanguage, LanguageType requistedLanguageType);
    }
}