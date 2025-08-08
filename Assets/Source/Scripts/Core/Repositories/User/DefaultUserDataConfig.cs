using Source.Scripts.Core.Repositories.User.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.User
{
    internal sealed class DefaultUserDataConfig : ScriptableObject, IDefaultUserDataConfig
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AssetReferenceT<Sprite> Icon { get; private set; }
    }
}