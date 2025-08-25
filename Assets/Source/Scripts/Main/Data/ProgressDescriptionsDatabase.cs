using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;
using UnityEngine;

namespace Source.Scripts.Main.Data
{
    internal sealed class ProgressDescriptionsDatabase : ScriptableObject, IProgressDescriptionsDatabase
    {
        [field: SerializeField]
        public EnumArray<ProgressDescriptionType, ProgressDescriptionData> Descriptions { get; private set; } =
            new(EnumMode.SkipFirst);

        [field: SerializeField] public RandomInt LowMediumTransitionRandom { get; private set; }
        [field: SerializeField] public RandomInt MediumHighTransitionRandom { get; private set; }
        [field: SerializeField] public RandomInt DefaultRandomPercent { get; private set; }
    }
}