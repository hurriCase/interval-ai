using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Other;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Loader
{
    [Preserve]
    internal class AddressablesLoader : IAddressablesLoader
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
    }
}