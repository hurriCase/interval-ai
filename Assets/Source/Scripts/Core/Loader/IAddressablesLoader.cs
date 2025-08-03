using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Source.Scripts.Core.Loader
{
    internal interface IAddressablesLoader
    {
        UniTask<T> LoadAsync<T>(AssetReference assetReference, CancellationToken token)
            where T : Object;

        UniTask<T> LoadAsync<T>(string assetGuid, CancellationToken token)
            where T : Object;

        UniTask<TComponent> LoadComponentAsync<TComponent>(
            AssetReference assetReference,
            CancellationToken token)
            where TComponent : Component;

        UniTask AssignImageAsync(Image image, AssetReference assetReference, CancellationToken token);
    }
}