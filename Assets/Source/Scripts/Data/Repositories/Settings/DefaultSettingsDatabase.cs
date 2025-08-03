using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Data.Repositories.Settings
{
    [CreateAssetMenu(fileName = nameof(DefaultSettingsDatabase), menuName = nameof(DefaultSettingsDatabase))]
    internal sealed class DefaultSettingsDatabase : ScriptableObject, IDefaultSettingsDatabase
    {
        [field: SerializeField]
        public EnumArray<Language, AssetReferenceT<Sprite>> LanguageSprites { get; private set; } =
            new(EnumMode.SkipFirst);
        [field: SerializeField] public List<CooldownByDate> Cooldowns { get; private set; }
        [field: SerializeField] public int DailyGoal { get; private set; }
    }
}