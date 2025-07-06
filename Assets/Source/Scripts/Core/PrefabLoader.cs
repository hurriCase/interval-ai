using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Helpers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core
{
    internal static class PrefabLoader
    {
        internal static async UniTask<T> LoadAsync<T>(AssetReference assetReference, CancellationToken token) where T : Object
        {
            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loading {typeof(T).Name}...");

#if ADDRESSABLES_LOG_ALL
            using var stopWatchScope = AddressablesLogger.LogWithTimePast("[PrefabLoader::LoadAsync]");
#endif
            var asset = await Addressables.LoadAssetAsync<T>(assetReference).WithCancellation(token);

            AddressablesLogger.Log($"[PrefabLoader::LoadAsync] Loaded '{asset.name}' ({typeof(T).Name})");
            return asset;
        }
    }
}