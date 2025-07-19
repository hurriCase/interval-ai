using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    [Resource(ResourcePaths.ResourcePath, nameof(DefaultWordsDatabase))]
    internal sealed class DefaultWordsDatabase : SingletonScriptableObject<DefaultWordsDatabase>
    {
        [field: SerializeField] internal List<WordEntry> WordEntries { get; private set; }
    }
}