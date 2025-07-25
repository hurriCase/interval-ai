using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Progress
{
    [Resource(ResourcePaths.DatabaseFullPath, nameof(WordGoalDatabase), ResourcePaths.DatabaseResourcePath)]
    internal sealed class WordGoalDatabase : SingletonScriptableObject<WordGoalDatabase>
    {
        [field: SerializeField] internal List<int> DefaultWordGoals { get; private set; }
    }
}