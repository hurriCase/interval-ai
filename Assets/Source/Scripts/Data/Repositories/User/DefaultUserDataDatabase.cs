using System.Collections.Generic;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using Source.Scripts.Data.Repositories.User.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Data.Repositories.User
{
    internal sealed class DefaultUserDataDatabase : ScriptableObject, IDefaultUserDataDatabase
    {
        [field: SerializeField] public List<CooldownByDate> DefaultCooldowns { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AssetReferenceT<Sprite> Icon { get; private set; }
    }
}