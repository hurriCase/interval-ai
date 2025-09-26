using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    [CreateAssetMenu(
        fileName = nameof(ProgressColorMapping),
        menuName = MenuPaths.MappingsPath + nameof(ProgressColorMapping)
    )]
    internal sealed class ProgressColorMapping : ThemeStateMappingGeneric<LearningState> { }
}