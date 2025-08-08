using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IDefaultSettingsConfig
    {
        EnumArray<PracticeState, LearningState[]> PracticeToLearningStates { get; }
        EnumArray<LanguageLevel, AssetReferenceT<Sprite>> LevelLanguageIcons { get; }
        EnumArray<Language, AssetReferenceT<Sprite>> LanguageSprites { get; }
        Language DefaultLearningLanguage { get; }
        Language AdditionalDefaultLanguage { get; }
        List<CooldownByDate> Cooldowns { get; }
        int DailyGoal { get; }
    }
}