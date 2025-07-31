using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Randoms;
using Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress;

namespace Source.Scripts.Main.Data.Base
{
    internal interface IProgressDescriptionsDatabase
    {
        List<ProgressDescriptionData> DescriptionLocalizations { get; }
        RandomInt LowMediumTransitionRandom { get; }
        RandomInt MediumHighTransitionRandom { get; }
        RandomInt DefaultRandomPercent { get; }
    }
}