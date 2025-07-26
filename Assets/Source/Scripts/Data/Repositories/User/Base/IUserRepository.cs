using System.Collections.Generic;
using System.Globalization;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Data.Repositories.User.Base
{
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<CachedSprite> UserIcon { get; }
        PersistentReactiveProperty<LanguageLevel> UserLevel { get; }
        PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }
    }
}