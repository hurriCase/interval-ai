using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.References.Base
{
    internal interface ISpriteReferences
    {
        EnumArray<LanguageLevel, AssetReferenceSprite> LevelLanguageIcons { get; }
        EnumArray<SystemLanguage, AssetReferenceSprite> LanguageSprites { get; }
        List<AssetReferenceSprite> CategoryIcons { get; }
    }
}