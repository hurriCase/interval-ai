using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Data.Repositories.User.Base
{
    internal interface IDefaultUserDataDatabase
    {
        string Name { get; }
        AssetReferenceT<Sprite> Icon { get; }
    }
}