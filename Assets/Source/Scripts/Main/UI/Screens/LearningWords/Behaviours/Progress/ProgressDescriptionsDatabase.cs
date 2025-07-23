using System.Collections.Generic;
using CustomUtils.Runtime.AssetLoader;
using CustomUtils.Runtime.CustomTypes.Randoms;
using CustomUtils.Runtime.CustomTypes.Singletons;
using Source.Scripts.Data;
using Source.Scripts.Main.Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.Idon_tknow
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