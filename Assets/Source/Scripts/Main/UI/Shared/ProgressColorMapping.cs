using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared
{
    [CreateAssetMenu(fileName = nameof(ProgressColorMapping), menuName = nameof(ProgressColorMapping))]
    internal sealed class ProgressColorMapping : ThemeStateMappingGeneric<LearningState> { }
}