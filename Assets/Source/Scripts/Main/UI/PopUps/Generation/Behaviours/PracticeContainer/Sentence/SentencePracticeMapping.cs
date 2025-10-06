using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Localization.LocalizationTypes;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.PracticeContainer.Sentence
{
    [CreateAssetMenu(fileName = nameof(SentencePracticeMapping), menuName = nameof(SentencePracticeMapping))]
    internal sealed class SentencePracticeMapping : ThemeStateMappingGeneric<SentencePracticeState> { }
}