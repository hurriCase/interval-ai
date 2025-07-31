using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using UnityEngine;

namespace Source.Scripts.Main.Data
{
    internal sealed class ProgressDescriptionsDatabase : ScriptableObject, IProgressDescriptionsDatabase
    {
        [field: SerializeField] public List<ProgressDescriptionData> DescriptionLocalizations { get; private set; }
        [field: SerializeField] public RandomInt LowMediumTransitionRandom { get; private set; }
        [field: SerializeField] public RandomInt MediumHighTransitionRandom { get; private set; }
        [field: SerializeField] public RandomInt DefaultRandomPercent { get; private set; }
    }
}