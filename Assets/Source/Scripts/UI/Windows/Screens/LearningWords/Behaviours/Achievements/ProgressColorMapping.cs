using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Data.Repositories.Entries.Words;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements
{
    [CreateAssetMenu(fileName = nameof(ProgressColorMapping), menuName = nameof(ProgressColorMapping))]
    internal sealed class ProgressColorMapping : ThemeStateMappingGeneric<LearningState> { }
}