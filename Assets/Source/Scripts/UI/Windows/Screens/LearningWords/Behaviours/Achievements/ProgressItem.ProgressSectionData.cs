using System;
using CustomUtils.Runtime.UI;
using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.Data.Repositories.Entries.Words;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements
{
    internal sealed partial class ProgressItem
    {
        [Serializable]
        internal struct ProgressSectionData
        {
            [field: SerializeField] internal RoundedFilledImageComponent RoundedFilledImage { get; private set; }
            [field: SerializeField] internal ImageThemeComponent ImageTheme { get; private set; }
            [field: SerializeField] internal LearningState LearningState { get; private set; }
        }
    }
}