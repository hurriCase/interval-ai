using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Data.Repositories.User.Base
{
    internal interface IDefaultUserDataDatabase
    {
        List<CooldownByDate> DefaultCooldowns { get; }
        string Name { get; }
        AssetReferenceT<Sprite> Icon { get; }
    }
}