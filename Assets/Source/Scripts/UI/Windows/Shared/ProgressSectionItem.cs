using CustomUtils.Runtime.UI;
using CustomUtils.Runtime.UI.Theme.Components;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    internal sealed class ProgressSectionItem : MonoBehaviour
    {
        [field: SerializeField] internal RoundedFilledImageComponent RoundedFilledImage { get; private set; }
        [field: SerializeField] internal ImageThemeComponent ImageTheme { get; private set; }
    }
}