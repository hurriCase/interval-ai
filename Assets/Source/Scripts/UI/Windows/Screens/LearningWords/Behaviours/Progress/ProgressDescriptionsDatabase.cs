using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Randoms;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress
{
    [Resource(ResourcePaths.DatabaseFullPath, nameof(ProgressDescriptionsDatabase), ResourcePaths.DatabaseResourcePath)]
    internal sealed class ProgressDescriptionsDatabase : SingletonScriptableObject<ProgressDescriptionsDatabase>
    {
        [field: SerializeField] internal List<ProgressDescriptionData> DescriptionLocalizations { get; private set; }
        [field: SerializeField] internal RandomInt LowMediumTransitionRandom { get; private set; }
        [field: SerializeField] internal RandomInt MediumHighTransitionRandom { get; private set; }
        [field: SerializeField] internal RandomInt DefaultRandomPercent { get; private set; }
    }
}