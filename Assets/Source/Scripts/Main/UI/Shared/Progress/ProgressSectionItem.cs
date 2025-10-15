using CustomUtils.Runtime.UI.CustomComponents.FilledImage;
using CustomUtils.Runtime.UI.Theme;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class ProgressSectionItem : MonoBehaviour
    {
        [field: SerializeField] internal RoundedFilledImage RoundedFilledImage { get; private set; }
        [field: SerializeField] internal ThemeComponent ImageTheme { get; private set; }
    }
}