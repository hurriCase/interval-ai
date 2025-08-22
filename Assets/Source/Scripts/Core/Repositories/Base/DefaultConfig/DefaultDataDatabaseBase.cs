using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MemoryPack;
using Source.Scripts.Core.Loader;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Source.Scripts.Core.Repositories.Base.DefaultConfig
{
    internal abstract class DefaultDataDatabaseBase<TEntry> : ScriptableObject, IDefaultDataDatabase
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