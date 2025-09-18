using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using R3;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface ILanguageSettingsRepository
    {
        PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; }
        ReadOnlyReactiveProperty<EnumArray<LanguageType, SystemLanguage>> LanguageByType { get; }
        PersistentReactiveProperty<SystemLanguage> SystemLanguage { get; }
        PersistentReactiveProperty<LanguageType> FirstShowLanguageType { get; }
        PersistentReactiveProperty<LanguageType> CardLearnLanguageType { get; }
        PersistentReactiveProperty<LanguageType> CardReviewLanguageType { get; }
        EnumArray<LanguageType, ReactiveProperty<SystemLanguage>> LanguageProperties { get; }
        void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType);
        SystemLanguage GetOppositeLanguage(SystemLanguage language);
    }
}