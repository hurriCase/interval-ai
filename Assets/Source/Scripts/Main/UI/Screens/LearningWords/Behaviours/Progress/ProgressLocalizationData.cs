using System;
using CustomUtils.Runtime.Localization;
using UnityEngine;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    [Serializable]
    internal struct ProgressLocalizationData
    {
        [field: SerializeField] internal LocalizationKey TitleKey { get; private set; }
        [field: SerializeField] internal LocalizationKey ProgressDescriptionKey { get; private set; }
    }
}