using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Sprites;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Source.Scripts.Core.Loader
{
    internal sealed class AddressablesLoader : IAddressablesLoader
    {
        public async UniTask<T> LoadAsync<T>(AssetReference assetReference, CancellationToken token)
            where T : Object =>
            await LoadAsync<T>(assetReference.AssetGUID, token);

        public async UniTask<T> LoadAsync<T>(string assetGuid, CancellationToken token) where T : Object
        {
            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loading {typeof(T).Name}...");

#if ADDRESSABLES_LOG_ALL
            using var stopWatchScope = AddressablesLogger.LogWithTimePast("[PrefabLoader::LoadAsync]");
#endif
            var asset = await Addressables.LoadAssetAsync<T>(assetGuid).WithCancellation(token);

            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loaded '{asset.name}' ({typeof(T).Name})");
            return asset;
        }

        public async UniTask<TComponent> LoadComponentAsync<TComponent>(
            AssetReference assetReference,
            CancellationToken token)
            where TComponent : Component
        {
            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loading {typeof(TComponent).Name}...");

#if ADDRESSABLES_LOG_ALL
            using var stopWatchScope = AddressablesLogger.LogWithTimePast("[PrefabLoader::LoadAsync]");
#endif
            var asset = await Addressables.LoadAssetAsync<GameObject>(assetReference).WithCancellation(token);

            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loaded '{asset.name}' ({typeof(TComponent).Name})");
            return asset.GetComponent<TComponent>();
        }

        public async UniTask AssignImageAsync(Image image, AssetReference assetReference, CancellationToken token)
            => image.sprite = await LoadAsync<Sprite>(assetReference, token);

        public async UniTask AssignImageAsync(Image image, CachedSprite cachedSprite, CancellationToken token)
        {
            if (cachedSprite.IsValid)
                image.sprite = await LoadAsync<Sprite>(cachedSprite.AssetGUID, token);
        }
    }
}