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
    internal interface IUserRepository
    {
        PersistentReactiveProperty<string> Nickname { get; }
        PersistentReactiveProperty<CultureInfo> CurrentCulture { get; }
        PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; }
        PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; }
        PersistentReactiveProperty<Sprite> UserIcon { get; }
        PersistentReactiveProperty<LanguageLevel> UserLevel { get; }
        PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; }
    }
}