using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Core.Repositories.User.Base
{
    internal interface IDefaultUserDataDatabase
    {
        string Name { get; }
        AssetReferenceT<Sprite> Icon { get; }
    }
}