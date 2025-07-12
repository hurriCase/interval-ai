using System;
using CustomUtils.Runtime.UI;
using CustomUtils.Runtime.UI.Theme.Components;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    [Serializable]
    internal struct ProgressSectionData
    {
        [field: SerializeField] internal RoundedFilledImageComponent RoundedFilledImage { get; private set; }
        [field: SerializeField] internal ImageThemeComponent ImageTheme { get; private set; }
    }
}