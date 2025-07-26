using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;

namespace Source.Scripts.Data.Repositories.Settings.Base
{
    internal interface ISettingsRepository
    {
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }
        PersistentReactiveProperty<int> DailyGoal { get; }
    }
}