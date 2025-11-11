using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Others;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Exercises.Behaviours.PracticeContainer.Sentence
{
    [CreateAssetMenu(
        fileName = nameof(SentencePracticeMapping),
        menuName = MenuPaths.MappingsPath + nameof(SentencePracticeMapping)
    )]
    internal sealed class SentencePracticeMapping : ThemeStateMappingGeneric<SentencePracticeState> { }
}