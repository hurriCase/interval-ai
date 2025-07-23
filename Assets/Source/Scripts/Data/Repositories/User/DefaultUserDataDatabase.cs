using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data.Repositories.Vocabulary;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.User
{
    [Resource(ResourcePaths.DatabaseFullPath, nameof(DefaultUserDataDatabase), ResourcePaths.DatabaseResourcePath)]
    internal sealed class DefaultUserDataDatabase : SingletonScriptableObject<DefaultUserDataDatabase>
    {
        [field: SerializeField] internal List<CooldownByDate> DefaultCooldowns { get; private set; }
        [field: SerializeField] internal string Name { get; private set; }
        [field: SerializeField] internal Sprite Icon { get; private set; }
    }
}