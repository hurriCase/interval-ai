using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data.Repositories.Entries;
using UnityEngine;

namespace Source.Scripts.Data
{
    [Resource(ResourcePaths.ResourcePath, nameof(DefaultCategoriesDatabase))]
    internal sealed class DefaultCategoriesDatabase : SingletonScriptableObject<DefaultCategoriesDatabase>
    {
        [field: SerializeField] internal List<CategoryEntry> Categories { get; private set; }
    }
}