using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Core;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.LearningStarts.GraphProgress
{
    [Resource(ResourcePaths.ResourcePath, nameof(ProgressGraphSettings))]
    internal sealed class ProgressGraphSettings : SingletonScriptableObject<ProgressGraphSettings>
    {
        [field: SerializeField] internal List<GraphProgressRange> GraphProgressRanges { get; private set; }
        [field: SerializeField] internal int GraphPointsCount { get; private set; }
    }
}