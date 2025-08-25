using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Randoms;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    [Serializable]
    internal struct ProgressDescriptionData
    {
        [field: SerializeField] internal RandomInt Percent { get; private set; }
        [field: SerializeField] internal List<ProgressLocalizationData> ProgressLocalizationData { get; private set; }
    }
}