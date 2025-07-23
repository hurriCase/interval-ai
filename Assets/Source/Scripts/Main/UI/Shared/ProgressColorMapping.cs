using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Shared
{
    [CreateAssetMenu(fileName = nameof(ProgressColorMapping), menuName = nameof(ProgressColorMapping))]
    internal sealed class ProgressColorMapping : ThemeStateMappingGeneric<LearningState> { }
}