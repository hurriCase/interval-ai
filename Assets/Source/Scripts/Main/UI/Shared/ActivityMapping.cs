using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Shared
{
    [CreateAssetMenu(fileName = nameof(ActivityMapping), menuName = nameof(ActivityMapping))]
    internal sealed class ActivityMapping : ThemeStateMappingGeneric<ActivityState> { }
}