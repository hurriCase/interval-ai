using System;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    [Serializable]
    internal struct ProgressLocalizationData
    {
        [field: SerializeField] internal string TitleKey { get; private set; }
        [field: SerializeField] internal string ProgressDescriptionKey { get; private set; }
    }
}