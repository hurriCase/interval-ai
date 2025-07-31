using System;
using CustomUtils.Runtime.CustomTypes.Randoms;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    [Serializable]
    internal struct ProgressDescriptionData
    {
        [field: SerializeField] internal string TitleKey { get; private set; }
        [field: SerializeField] internal string ProgressDescriptionKey { get; private set; }
        [field: SerializeField] internal ProgressDescriptionType Type { get; private set; }
        [field: SerializeField] internal RandomInt Percent { get; private set; }
    }
}