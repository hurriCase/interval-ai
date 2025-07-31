using MemoryPack;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Others
{
    [MemoryPackable]
    internal readonly partial struct CachedSprite
    {
        public string AssetGUID { get; }

        [MemoryPackConstructor]
        public CachedSprite(string assetGUID) => AssetGUID = assetGUID;

        public CachedSprite(AssetReference assetReference) => AssetGUID = assetReference.AssetGUID;

        public bool IsValid => string.IsNullOrEmpty(AssetGUID) is false;
    }
}