using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.References.Base
{
    internal interface ISpriteReferences
    {
        EnumArray<LanguageLevel, AssetReferenceT<Sprite>> LevelLanguageIcons { get; }
        EnumArray<SystemLanguage, AssetReferenceT<Sprite>> LanguageSprites { get; }
    }
}