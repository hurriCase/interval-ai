using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.References
{
    internal sealed class SpriteReferences : ScriptableObject, ISpriteReferences
    {
        [field: SerializeField]
        public EnumArray<LanguageLevel, AssetReferenceSprite> LevelLanguageIcons { get; private set; }

        [field: SerializeField]
        public EnumArray<SystemLanguage, AssetReferenceSprite> LanguageSprites { get; private set; }

        [field: SerializeField] public List<AssetReferenceSprite> CategoryIcons { get; set; }
    }
}