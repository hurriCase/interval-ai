using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class DefaultSettingsConfig : ScriptableObject, IDefaultSettingsConfig
    {
        [field: SerializeField]
        public EnumArray<LanguageLevel, AssetReferenceT<Sprite>> LevelLanguageIcons { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<PracticeState, LearningState[]> PracticeToLearningStates { get; private set; }
            = new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<Language, AssetReferenceT<Sprite>> LanguageSprites { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField] public Language DefaultLearningLanguage { get; private set; }
        [field: SerializeField] public Language AdditionalDefaultLanguage { get; private set; }
        [field: SerializeField] public List<CooldownByDate> Cooldowns { get; private set; }
        [field: SerializeField] public int DailyGoal { get; private set; }
    }
}