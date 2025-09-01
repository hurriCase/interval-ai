using CustomUtils.Runtime.UI.CustomComponents;
using CustomUtils.Runtime.UI.Theme.Components;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class ProgressSectionItem : MonoBehaviour
    {
        [field: SerializeField] internal RoundedFilledImageComponent RoundedFilledImage { get; private set; }
        [field: SerializeField] internal ImageThemeComponent ImageTheme { get; private set; }
    }
}