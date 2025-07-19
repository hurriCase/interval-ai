using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    [Resource(ResourcePaths.ResourcePath, nameof(DefaultCooldownsDatabase))]
    internal sealed class DefaultCooldownsDatabase : SingletonScriptableObject<DefaultCooldownsDatabase>
    {
        [field: SerializeField] internal List<CooldownData> DefaultCooldowns { get; private set; }
    }
}