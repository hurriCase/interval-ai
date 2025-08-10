using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Sprites
{
    internal sealed class SpriteReferences : ScriptableObject, ISpriteReferences
    {
        [field: SerializeField]
        public EnumArray<LanguageLevel, AssetReferenceT<Sprite>> LevelLanguageIcons { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField]
        public EnumArray<SystemLanguage, AssetReferenceT<Sprite>> LanguageSprites { get; private set; } =
            new(EnumMode.Default);
    }
}