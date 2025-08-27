using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.AddressableSystem;
using Cysharp.Threading.Tasks;
using MemoryPack;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Source.Scripts.Core.Repositories.Base.DefaultConfig
{
    internal abstract class DefaultDataDatabaseBase<TEntry> : ScriptableObject, IDefaultDataDatabase
    {
        [SerializeField] private AssetReferenceT<TextAsset> _defaultAssetRereference;

        private IAddressablesLoader _addressablesLoader;

        [Inject]
        internal void Inject(IAddressablesLoader addressablesLoader)
        {
            _addressablesLoader = addressablesLoader;
        }

        public List<TEntry> Defaults { get; private set; }

        public async UniTask InitAsync(CancellationToken token)
        {
            var bytesAsset =
                await _addressablesLoader.LoadAsync<TextAsset>(_defaultAssetRereference, token);

            Defaults = MemoryPackSerializer.Deserialize<List<TEntry>>(bytesAsset.bytes);
        }
    }
}