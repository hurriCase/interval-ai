using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Core.Others;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Activity
{
    [CreateAssetMenu(fileName = nameof(ActivityMapping), menuName = MenuPaths.MappingsPath + nameof(ActivityMapping))]
    internal sealed class ActivityMapping : ThemeStateMappingGeneric<ActivityState> { }
}