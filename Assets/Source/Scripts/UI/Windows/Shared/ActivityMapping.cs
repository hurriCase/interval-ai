using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    [CreateAssetMenu(fileName = nameof(ActivityMapping), menuName = nameof(ActivityMapping))]
    internal sealed class ActivityMapping : ThemeStateMappingGeneric<ActivityState> { }
}