using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Core;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    [Resource(ResourcePaths.ResourcePath, nameof(ProgressGraphSettings))]
    internal sealed class ProgressGraphSettings : SingletonScriptableObject<ProgressGraphSettings>
    {
        [field: SerializeField] internal List<DateRange> GraphProgressRanges { get; private set; }
        [field: SerializeField] internal int GraphPointsCount { get; private set; }
    }
}