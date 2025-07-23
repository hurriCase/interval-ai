using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    [Resource(ResourcePaths.DatabaseFullPath, nameof(DefaultCategoriesDatabase), ResourcePaths.DatabaseResourcePath)]
    internal sealed class DefaultCategoriesDatabase : SingletonScriptableObject<DefaultCategoriesDatabase>
    {
        [field: SerializeField] internal List<CategoryEntry> Categories { get; private set; }
    }
}