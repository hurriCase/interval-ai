using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;

namespace Source.Scripts.Main.Data.Base
{
    internal interface IProgressDescriptionsDatabase
    {
        EnumArray<ProgressDescriptionType, ProgressDescriptionData> Descriptions { get; }
        RandomInt LowMediumTransitionRandom { get; }
        RandomInt MediumHighTransitionRandom { get; }
        RandomInt DefaultRandomPercent { get; }
    }
}