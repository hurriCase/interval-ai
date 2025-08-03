using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IDefaultSettingsDatabase
    {
        EnumArray<Language, AssetReferenceT<Sprite>> LanguageSprites { get; }
        List<CooldownByDate> Cooldowns { get; }
        int DailyGoal { get; }
    }
}