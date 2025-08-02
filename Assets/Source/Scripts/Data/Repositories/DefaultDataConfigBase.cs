using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MemoryPack;
using Source.Scripts.Core.Loader;
using Source.Scripts.Core.Repositories;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Source.Scripts.Data.Repositories
{
    internal abstract class DefaultDataConfigBase<TEntry> : ScriptableObject, IDefaultConfig
    {
        [SerializeField] private AssetReferenceT<TextAsset> _defaultAssetRereference;

        [Inject] private IAddressablesLoader _addressablesLoader;
        public List<TEntry> Defaults { get; private set; }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var bytesAsset =
                await _addressablesLoader.LoadAsync<TextAsset>(_defaultAssetRereference, cancellationToken);

            Defaults = MemoryPackSerializer.Deserialize<List<TEntry>>(bytesAsset.bytes);
        }
    }
}