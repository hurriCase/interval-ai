using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.User.Base
{
    internal interface IDefaultUserDataConfig
    {
        string Name { get; }
        AssetReferenceT<Sprite> Icon { get; }
    }
}